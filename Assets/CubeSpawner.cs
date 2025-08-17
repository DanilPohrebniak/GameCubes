using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    [Header("Hand Settings")]
    public Transform hand;
    public float handPushDistance = 0.3f;
    public float handPushDuration = 0.2f;

    [Header("References")]
    public DiceManager diceManager;

    private bool isRolling = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isRolling)
        {
            StartCoroutine(ThrowWithHand());
        }
    }

    public IEnumerator ThrowWithHand()
    {
        isRolling = true;

        // Анимация руки
        if (hand != null)
            yield return StartCoroutine(PushHand());

        // Создание и бросок кубиков
        diceManager.SpawnAndThrow(transform);

        // Сообщаем GameManager, что начался новый бросок
        GameManager.Instance.OnDiceThrown();

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
