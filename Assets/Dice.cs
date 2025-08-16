using UnityEngine;

public class Dice : MonoBehaviour
{
    [Header("Face Anchors")]
    public Transform[] faceAnchors; // 6 пустых объектов, привязанных к граням кубика

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool IsStopped()
    {
        return rb.IsSleeping();
    }

    public int GetValue()
    {
        if (!IsStopped()) return 0; // если кубик ещё крутится — 0

        Transform bestFace = null;
        float maxDot = -1f;

        foreach (var face in faceAnchors)
        {
            float dot = Vector3.Dot(face.up, Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestFace = face;
            }
        }

        return bestFace != null ? bestFace.GetSiblingIndex() + 1 : 0; // индекс+1 = число
    }
}
