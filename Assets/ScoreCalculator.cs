using System.Linq;

public static class ScoreCalculator
{
    public static int CalculateScore(int[] diceValues)
    {
        int score = 0;
        int[] counts = new int[7]; // индексы 1–6

        foreach (int v in diceValues)
            counts[v]++;

        // Проверка на комбинацию 1-6
        bool allNumbers = true;
        for (int i = 1; i <= 6; i++)
            if (counts[i] == 0) allNumbers = false;

        if (allNumbers) return 1500;
        if (!allNumbers && counts[1] == 0) return 750;
        if (!allNumbers && counts[6] == 0) return 500;

        // Тройки и больше
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] >= 3)
            {
                int baseScore = (i == 1) ? 1000 : i * 100;
                int extra = counts[i] - 3;
                score += baseScore * (int)System.Math.Pow(2, extra);
                counts[i] = 0; // обнуляем, чтобы не учитывать эти кубики снова
            }
        }

        // Единицы и пятерки
        score += counts[1] * 100;
        score += counts[5] * 50;

        return score;
    }
}
