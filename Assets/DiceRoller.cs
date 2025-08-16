using UnityEngine;
using System.Collections.Generic;

public class DiceRoller : MonoBehaviour
{
    public GameObject dicePrefab;     // Префаб кубика
    public Transform spawnPoint;      // Где спавнить кубик
    public int diceCount = 2;         // Количество кубиков

    private List<GameObject> activeDice = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))  // Нажал F → бросить кубики
        {
            RollDice();
        }
    }

    void RollDice()
    {
        // Удаляем старые кубики
        foreach (var dice in activeDice)
        {
            Destroy(dice);
        }
        activeDice.Clear();

        // Создаём новые кубики
        for (int i = 0; i < diceCount; i++)
        {
            Vector3 spawnPos = spawnPoint.position + new Vector3(i * 1.5f, 0, 0); // чуть сдвигаем
            GameObject newDice = Instantiate(dicePrefab, spawnPos, Random.rotation);
            Rigidbody rb = newDice.GetComponent<Rigidbody>();
            rb.AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse); // пинок
            activeDice.Add(newDice);
        }
    }
}
