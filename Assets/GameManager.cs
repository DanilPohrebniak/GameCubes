using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // ✅ Синглтон

    public CubeSpawner spawner;
    public DiceManager diceManager;

    private int currentPlayer = 1;
    private int[] scores = new int[2];

    private bool throwInProgress = false; // ✅ добавили флаг

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            Debug.Log($"Игрок {currentPlayer}, нажмите кнопку для броска!");

            // Ждём, пока игрок начнет бросок
            yield return new WaitUntil(() => throwInProgress);

            // Ждём пока появятся кубики
            yield return new WaitUntil(() => diceManager.Dices.Count > 0);

            // Ждём пока кубики остановятся
            yield return new WaitUntil(() => diceManager.AllStopped());

            // Получаем значения кубиков
            int[] values = diceManager.Dices.Select(d => d.GetValue()).ToArray();
            int result = ScoreCalculator.CalculateScore(values);

            scores[currentPlayer - 1] += result;

            Debug.Log($"Игрок {currentPlayer} выбросил {string.Join(",", values)}. Очки: {scores[0]} - {scores[1]}");

            // Очистка кубиков (если нужно)
            //foreach (var dice in diceManager.Dices)
            //    if (dice != null) Destroy(dice.gameObject);
            //diceManager.Clear();

            // Сброс флага
            throwInProgress = false;

            // Меняем игрока
            currentPlayer = (currentPlayer == 1) ? 2 : 1;
        }
    }

    // 🚩 Этот метод вызывает CubeSpawner, когда начинается бросок
    public void OnDiceThrown()
    {
        throwInProgress = true;
    }
}
