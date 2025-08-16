using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    readonly List<Dice> dice = new List<Dice>();

    public void Register(Dice d)
    {
        if (d && !dice.Contains(d)) dice.Add(d);
    }

    public void ClearAll()
    {
        dice.Clear();
    }

    public IEnumerator WaitUntilAllSettled()
    {
        // Ждём, пока у всех IsSettled = true
        while (true)
        {
            if (dice.Count > 0 && dice.All(d => d != null && d.IsSettled)) break;
            yield return null;
        }
    }

    public int[] GetValues()
    {
        return dice.Where(d => d != null).Select(d => d.CurrentValue).ToArray();
    }
}
