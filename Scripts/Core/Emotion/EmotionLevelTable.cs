using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Core;

public struct EmotionLevelRow
{
    public int Level;
    public int MaxEmotion;
    public int CostCap;
    public int SpeedDiceCount;
    public int SpeedBonus;
    public int AtkDefBonus;
    public int CostRecovery;
    public int DrawCount;

    public EmotionLevelRow(int level, int maxEmotion, int costCap, int speedDiceCount,
        int speedBonus, int atkDefBonus, int costRecovery, int drawCount)
    {
        Level = level;
        MaxEmotion = maxEmotion;
        CostCap = costCap;
        SpeedDiceCount = speedDiceCount;
        SpeedBonus = speedBonus;
        AtkDefBonus = atkDefBonus;
        CostRecovery = costRecovery;
        DrawCount = drawCount;
    }
}

public static class EmotionLevelTable
{
    private static readonly List<EmotionLevelRow> Rows = new()
    {
        new (0, 3, 3, 1, 0, 0, 1, 1),
        new (1, 9, 4, 1, 0, 1, 1, 1),
        new (2, 15, 5, 1, 1, 2, 1, 1),
        new (3, 21, 6, 2, 1, 4, 1, 2),
        new (4, 30, 7, 2, 2, 6, 1, 2),
        new (5, -1, 8, 3, 3, 10, 2, 3),
    };

    public static EmotionLevelRow Get(int level) =>
        Rows.FirstOrDefault(r => r.Level == level);

    public static bool CanLevelUp(int currentLevel, int accumulatedEmotion)
    {
        var row = Get(currentLevel);
        return row.MaxEmotion >= 0 && accumulatedEmotion >= row.MaxEmotion;
    }
}
