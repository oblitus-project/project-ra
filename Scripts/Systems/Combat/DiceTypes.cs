using System;

namespace ProjectRA.Combat;

public struct DiceRange
{
    public int Min;
    public int Max;

    public DiceRange(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int Roll(Random rng) => rng.Next(Min, Max + 1);
}

public struct OperationDice
{
    public OperationType Op;
    public DiceRange Variation;
    public bool UsePreviousAsBase;

    public int Apply(int baseValue, Random rng)
    {
        int varValue = Variation.Roll(rng);
        return Op switch
        {
            OperationType.Add => baseValue + varValue,
            OperationType.Subtract => baseValue - varValue,
            OperationType.Multiply => baseValue * varValue,
            OperationType.Divide => varValue != 0 ? baseValue / varValue : baseValue,
            _ => baseValue
        };
    }
}

public struct DiceInstance
{
    public DiceType Type;
    public DiceSubType SubType;
    public AttackAttribute Attribute;
    public AttackRange Range;
    public int BaseMin;
    public int BaseMax;
    public int LevelMod;
    public bool IsIndestructible;
    public int AwValue;
    public int HwValue;
    public OperationDice? Operation;
}
