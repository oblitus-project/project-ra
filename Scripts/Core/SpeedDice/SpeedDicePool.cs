using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Core;

public class SpeedDicePool
{
    public List<SpeedDice> Dice { get; private set; } = new();
    public int Count => Dice.Count;
    public int AvailableCount => Dice.Count(d => !d.IsDestroyed);

    public void Roll(int diceCount, int minSpeed, int maxSpeed, Random rng)
    {
        Dice.Clear();
        for (int i = 0; i < diceCount; i++)
            Dice.Add(new SpeedDice
            {
                Speed = rng.Next(minSpeed, maxSpeed + 1),
                IsDestroyed = false,
                IsCracked = false
            });
        Dice = Dice.OrderByDescending(d => d.Speed).ToList();
    }

    public void Destroy(int index)
    {
        if (index >= 0 && index < Dice.Count)
            Dice[index] = Dice[index] with { IsDestroyed = true };
    }

    public void Crack(int index)
    {
        if (index >= 0 && index < Dice.Count && !Dice[index].IsDestroyed)
            Dice[index] = Dice[index] with { IsCracked = true };
    }

    public void Reset()
    {
        for (int i = 0; i < Dice.Count; i++)
            Dice[i] = Dice[i] with { IsDestroyed = false, IsCracked = false };
    }
}
