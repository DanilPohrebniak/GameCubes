using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public List<Dice> dices = new List<Dice>();

    public void RegisterDice(Dice dice)
    {
        dices.Add(dice);
    }


    // Проверка, остановились ли все кубики
    public bool AllStopped()
    {
        return dices.All(d => d.IsStopped());
    }

    // Суммирует значения всех кубиков
    public int GetTotalValue()
    {
        return dices.Sum(d => d.GetValue());
    }

    // Очищает список кубиков перед новым броском
    public void Clear()
    {
        dices.Clear();
    }
}
