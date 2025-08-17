using UnityEngine;
using System.Collections.Generic;

public class DiceRoller : MonoBehaviour
{
    public GameObject dicePrefab;
    public Transform spawnPoint;
    public float throwForce = 80f;

    private List<GameObject> activeDice = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ClearDice();
            RollDice();
        }
    }

    void RollDice()
    {
        GameObject dice = Instantiate(dicePrefab, spawnPoint.position, Random.rotation);
        Rigidbody rb = dice.GetComponent<Rigidbody>();
        rb.AddTorque(Random.insideUnitSphere * throwForce, ForceMode.Impulse);

        activeDice.Add(dice);
    }

    void ClearDice()
    {
        foreach (var dice in activeDice)
        {
            if (dice != null) Destroy(dice);
        }
        activeDice.Clear();
    }
}
