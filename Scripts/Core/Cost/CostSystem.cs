using System;

namespace ProjectRA.Core;

public class CostSystem
{
    public int CurrentCost { get; private set; }
    public int MaxCost { get; private set; }
    public int EmotionLevel { get; private set; }

    public CostSystem()
    {
        var row = EmotionLevelTable.Get(0);
        MaxCost = row.CostCap;
        CurrentCost = MaxCost;
    }

    public void OnTurnStart()
    {
        var row = EmotionLevelTable.Get(EmotionLevel);
        CurrentCost = Math.Min(CurrentCost + row.CostRecovery, MaxCost);
    }

    public bool Spend(int amount)
    {
        if (CurrentCost < amount) return false;
        CurrentCost -= amount;
        return true;
    }

    public void OnEmotionLevelUp()
    {
        EmotionLevel++;
        var row = EmotionLevelTable.Get(EmotionLevel);
        MaxCost = row.CostCap;
        CurrentCost = MaxCost;
    }

    public void OnKillOrConfusion()
    {
        CurrentCost = Math.Min(CurrentCost + 1, MaxCost);
    }
}
