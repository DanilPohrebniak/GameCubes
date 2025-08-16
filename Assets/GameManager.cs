using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CubeSpawner spawner;
    public DiceManager diceManager;

    [Header("Игра")]
    public int playersCount = 2;
    public int targetScore = 3000;

    int currentPlayer = 0;
    int[] totalScores;

    void Start()
    {
        totalScores = new int[playersCount];
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            // 1) Бросаем
            yield return spawner.SpawnAndThrow();

            // 2) Ждём «стоп»
            yield return diceManager.WaitUntilAllSettled();

            // 3) Считаем
            var values = diceManager.GetValues();
            int gained = ScoreCalculator.CalculateMaxScore(values);

            // TODO: здесь можно вывести значения/очки на UI
            Debug.Log($"Player {currentPlayer + 1} rolled: [{string.Join(",", values)}] -> +{gained}");

            if (gained == 0)
            {
                // Сгорел — очки не добавляем
                Debug.Log($"Player {currentPlayer + 1} busts!");
            }
            else
            {
                totalScores[currentPlayer] += gained;
            }

            // Победа?
            if (totalScores[currentPlayer] >= targetScore)
            {
                Debug.Log($"Player {currentPlayer + 1} WINS with {totalScores[currentPlayer]} pts!");
                // Здесь можно показать UI победы и выйти из цикла
                yield break;
            }

            // Передаём ход
            currentPlayer = (currentPlayer + 1) % playersCount;

            // На минималках — просто следующая итерация (респавним новые кости).
            // Позже можно: удалять старые кубики, вызывать спавн заново, дать рероллы и выбор.
            yield return new WaitForSeconds(0.5f);
        }
    }
}
