using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [Tooltip("Индексы: 0->1, 1->2, ... 5->6")]
    public Transform[] faceAnchors = new Transform[6];

    public int CurrentValue { get; private set; } = 0;
    public bool IsSettled { get; private set; } = false;

    [Header("Порог успокоения")]
    [SerializeField] float linearThreshold = 0.05f;
    [SerializeField] float angularThreshold = 1.0f;
    [SerializeField] float stableTime = 0.25f;

    Rigidbody rb;
    float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Отслеживаем "успокоился ли" кубик
        if (rb.linearVelocity.magnitude < linearThreshold && rb.angularVelocity.magnitude < angularThreshold)
        {
            timer += Time.deltaTime;
            if (!IsSettled && timer >= stableTime)
            {
                IsSettled = true;
                EvaluateTopFace();
            }
        }
        else
        {
            // Движется — сбрасываем таймер и флаг
            timer = 0f;
            if (IsSettled) IsSettled = false;
        }
    }

    void EvaluateTopFace()
    {
        // Выбираем якорь с max dot(anch.up, worldUp)
        int best = -1;
        float bestDot = -999f;
        for (int i = 0; i < faceAnchors.Length; i++)
        {
            if (!faceAnchors[i]) continue;
            float d = Vector3.Dot(faceAnchors[i].up, Vector3.up);
            if (d > bestDot)
            {
                bestDot = d;
                best = i;
            }
        }
        CurrentValue = best >= 0 ? best + 1 : 0;
    }

    // Полезно при повторном броске
    public void ResetSettle()
    {
        IsSettled = false;
        timer = 0f;
        CurrentValue = 0;
    }
}
