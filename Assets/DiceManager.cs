using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject cubePrefab;
    public int cubeCount = 6;
    public float throwForce = 2f;
    public float spawnOffset = 0.5f;
    public float tableHalfWidth = 1.4f;

    private List<Dice> dices = new List<Dice>();
    public IReadOnlyList<Dice> Dices => dices;

    // Создание и бросок кубиков
    public void SpawnAndThrow(Transform spawnOrigin)
    {
        Clear();

        Vector3 forward = spawnOrigin.forward;
        Vector3 right = spawnOrigin.right;

        for (int i = 0; i < cubeCount; i++)
        {
            float sideOffset = Random.Range(-tableHalfWidth, tableHalfWidth);
            Vector3 spawnPos = spawnOrigin.position + right * sideOffset + forward * spawnOffset;

            GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
            cube.transform.localScale = Vector3.one * 0.33f;

            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 throwDir = forward + new Vector3(0f, Random.Range(-0.05f, 0.05f), 0f);
                rb.AddForce(throwDir.normalized * throwForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Impulse);
            }

            Dice dice = cube.GetComponent<Dice>();
            if (dice != null)
                dices.Add(dice);
        }
    }

    // Проверка, остановились ли все кубики
    public bool AllStopped() => dices.All(d => d.IsStopped());

    // Суммирует значения всех кубиков
    public int GetTotalValue() => dices.Sum(d => d.GetValue());

    // Удаляет все кубики из сцены и очищает список
    public void Clear()
    {
        foreach (Dice dice in dices)
        {
            if (dice != null)
                Destroy(dice.gameObject);
        }
        dices.Clear();
    }
}
