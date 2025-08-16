using System;
using System.Collections.Generic;
using System.Linq;

public static class ScoreCalculator
{
    // Главный метод: возвращает максимум очков для данного набора костей
    public static int CalculateMaxScore(int[] dice)
    {
        if (dice == null || dice.Length == 0) return 0;
        int[] counts = new int[7];
        foreach (var v in dice) if (v >= 1 && v <= 6) counts[v]++;

        // Мемоизация по ключу "c1..c6"
        var memo = new Dictionary<string, int>();
        return Best(counts, memo);
    }

    // Есть ли вообще очки (годится для проверки «сгорели/нет»)
    public static bool HasAnyScore(int[] dice)
    {
        return CalculateMaxScore(dice) > 0;
    }

    static int Best(int[] c, Dictionary<string, int> memo)
    {
        string key = $"{c[1]}{c[2]}{c[3]}{c[4]}{c[5]}{c[6]}";
        if (memo.TryGetValue(key, out int cached)) return cached;

        int best = 0;

        // 1) Стрит 1-6 (использует по 1 каждой)
        if (Enumerable.Range(1, 6).All(v => c[v] > 0))
        {
            var used = (int[])c.Clone();
            for (int v = 1; v <= 6; v++) used[v]--;
            best = Math.Max(best, 1500 + Best(used, memo));
        }

        // 2) Стрит без 1: (2..6)
        if (Enumerable.Range(2, 5).All(v => c[v] > 0))
        {
            var used = (int[])c.Clone();
            for (int v = 2; v <= 6; v++) used[v]--;
            best = Math.Max(best, 750 + Best(used, memo));
        }

        // 3) Стрит без 6: (1..5)
        if (Enumerable.Range(1, 5).All(v => c[v] > 0))
        {
            var used = (int[])c.Clone();
            for (int v = 1; v <= 5; v++) used[v]--;
            best = Math.Max(best, 500 + Best(used, memo));
        }

        // 4) Тройки+ с удвоением за каждую доп. кость
        for (int v = 1; v <= 6; v++)
        {
            if (c[v] >= 3)
            {
                int count = c[v];
                int baseScore = (v == 1) ? 1000 : v * 100; // три единицы = 1000, иначе X*100
                int extra = count - 3;
                int score = baseScore * (1 << extra); // удвоение за каждую доп. кость

                var used = (int[])c.Clone();
                used[v] = 0; // использовали все одинаковые кости целиком
                best = Math.Max(best, score + Best(used, memo));
            }
        }

        // 5) Одиночные 1 и 5 (по одной)
        if (c[1] > 0)
        {
            var used = (int[])c.Clone();
            used[1]--;
            best = Math.Max(best, 100 + Best(used, memo));
        }
        if (c[5] > 0)
        {
            var used = (int[])c.Clone();
            used[5]--;
            best = Math.Max(best, 50 + Best(used, memo));
        }

        memo[key] = best;
        return best;
    }
}
