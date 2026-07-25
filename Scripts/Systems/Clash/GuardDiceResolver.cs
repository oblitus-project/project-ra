namespace ProjectRA.Combat;

public static class GuardDiceResolver
{
    public enum GuardResult
    {
        ShieldGained,
        CounterConfusionDmg,
        EvadeSuccess,
        Consumed,
        TriggerCounter
    }

    public static GuardResult Resolve(DiceType guardDice, DiceType attackerDice)
    {
        return (guardDice, attackerDice) switch
        {
            (DiceType.Defensive, DiceType.Offensive) => GuardResult.ShieldGained,
            (DiceType.Defensive, _) => GuardResult.Consumed,

            (DiceType.Block, DiceType.Offensive) => GuardResult.CounterConfusionDmg,
            (DiceType.Block, DiceType.Defensive) => GuardResult.Consumed,
            (DiceType.Block, DiceType.Block) => GuardResult.CounterConfusionDmg,
            (DiceType.Block, DiceType.Evade) => GuardResult.Consumed,

            (DiceType.Evade, DiceType.Offensive) => GuardResult.EvadeSuccess,
            (DiceType.Evade, DiceType.Defensive) => GuardResult.Consumed,
            (DiceType.Evade, DiceType.Block) => GuardResult.Consumed,
            (DiceType.Evade, DiceType.Evade) => GuardResult.Consumed,

            _ => GuardResult.Consumed
        };
    }
}
