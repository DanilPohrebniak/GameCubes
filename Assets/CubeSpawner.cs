using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject cubePrefab;
    public int cubeCount = 6;
    public float throwForce = 2f;
    public float spawnOffset = 0.5f;
    public float tableHalfWidth = 1.4f;

    [Header("Hand Settings")]
    public Transform hand;
    public float handPushDistance = 0.3f;
    public float handPushDuration = 0.2f;

    [Header("References")]
    public DiceManager diceManager;

    private bool isRolling = false;

    void Update()
    {
        // Проверяем нажатие клавиши F и что бросок не выполняется
        if (Input.GetKeyDown(KeyCode.F) && !isRolling)
        {
            StartCoroutine(ThrowWithHand());
        }
    }

    public IEnumerator ThrowWithHand()
    {
        isRolling = true;

        // Очищаем старые кубики
        diceManager.Clear();

        // Анимация руки
        if (hand != null)
            yield return StartCoroutine(PushHand());

        // Создаём кубики
        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            float sideOffset = Random.Range(-tableHalfWidth, tableHalfWidth);
            Vector3 spawnPos = transform.position + right * sideOffset + forward * spawnOffset;

            GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
            cube.transform.localScale = Vector3.one * 0.33f;

            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 throwDir = forward + new Vector3(0f, Random.Range(-0.05f, 0.05f), 0f);
                rb.AddForce(throwDir.normalized * throwForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Impulse);
            }

            // Регистрируем кубик
            Dice dice = cube.GetComponent<Dice>();
            if (dice != null)
                diceManager.RegisterDice(dice);
        }

        isRolling = false;
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
