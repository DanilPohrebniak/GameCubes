using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public CubeSpawner spawner;
    public DiceManager diceManager;

    private int currentPlayer = 1;
    private int[] scores = new int[2];

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            Debug.Log($"Игрок {currentPlayer}, нажмите кнопку для броска!");

            // Ждём пока игрок бросит кубики
            yield return new WaitUntil(() => diceManager.dices.Count > 0);

            // Ждём пока кубики остановятся
            yield return new WaitUntil(() => diceManager.AllStopped());

            // Получаем значения кубиков
            int[] values = diceManager.dices.Select(d => d.GetValue()).ToArray();
            int result = ScoreCalculator.CalculateScore(values);

            scores[currentPlayer - 1] += result;

            Debug.Log($"Игрок {currentPlayer} выбросил {string.Join(",", values)}. Очки: {scores[0]} - {scores[1]}");

            // Уничтожаем кубики после броска
            foreach (var dice in diceManager.dices)
                if (dice != null)
                    Destroy(dice.gameObject);

            diceManager.Clear();

            // Меняем игрока
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }
    }
}
