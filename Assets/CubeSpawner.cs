using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    [Header("Кубики")]
    public GameObject cubePrefab;
    public int cubeCount = 6;
    public float throwForce = 2f;
    public float spawnOffset = 0.5f;
    public float tableHalfWidth = 1.4f;

    [Header("Рука")]
    public Transform hand;
    public float handPushDistance = 0.3f;
    public float handPushDuration = 0.2f;

    [Header("Связи")]
    public DiceManager diceManager;
    public bool autoStart = true;

    void Start()
    {
        if (autoStart) StartCoroutine(ThrowWithHand());
    }

    public IEnumerator SpawnAndThrow()
    {
        yield return StartCoroutine(ThrowWithHand());
    }

    IEnumerator ThrowWithHand()
    {
        if (diceManager) diceManager.ClearAll();

        // Анимация руки
        if (hand != null)
            yield return StartCoroutine(PushHand());

        // Спавн и бросок
        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            float sideOffset = Random.Range(-tableHalfWidth, tableHalfWidth);
            Vector3 spawnPos = transform.position + right * sideOffset + forward * spawnOffset;

            GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
            cube.transform.localScale = Vector3.one * 0.33f;

            // Регистрируем Dice
            var dice = cube.GetComponent<Dice>();
            if (diceManager && dice) diceManager.Register(dice);

            var rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 throwDir = forward + new Vector3(0f, Random.Range(-0.05f, 0.05f), 0f);
                rb.AddForce(throwDir.normalized * throwForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Impulse);
            }
        }
    }

    IEnumerator PushHand()
    {
        Vector3 startPos = hand.localPosition;
        Vector3 endPos = startPos + transform.forward * handPushDistance;

        float elapsed = 0f;
        while (elapsed < handPushDuration)
        {
            hand.localPosition = Vector3.Lerp(startPos, endPos, elapsed / handPushDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < handPushDuration)
        {
            hand.localPosition = Vector3.Lerp(endPos, startPos, elapsed / handPushDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        hand.localPosition = startPos;
    }
}
