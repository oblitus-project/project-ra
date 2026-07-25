using System;
using System.Threading.Tasks;
using ProjectRA.Entities;
using ProjectRA.Keyword;

namespace ProjectRA.Models;

public abstract class StatusEffectModel : AbstractModel
{
    public Creature Owner { get; internal set; }
    public EffectCategory Category { get; protected set; }
    public int MaxStack { get; protected set; } = int.MaxValue;
    public EffectTimingKeyword DecayTiming { get; protected set; } = EffectTimingKeyword.TurnEnd;
    public int DecayAmount { get; protected set; } = 1;
    public bool HasIntensity { get; protected set; }
    public bool HasStack { get; protected set; } = true;

    public int Stack { get; set; }
    public int Intensity { get; set; }
    public int RemainingTurns { get; set; }

    public void AddStack(int amount)
    {
        Stack = Math.Min(Stack + amount, MaxStack);
    }

    protected override void AfterCloned()
    {
        Stack = 0;
        Intensity = 0;
        RemainingTurns = 0;
        Owner = null;
    }

    public override async Task OnTurnEnd()
    {
        if (DecayTiming == EffectTimingKeyword.TurnEnd && DecayAmount > 0)
        {
            Stack = Math.Max(0, Stack - DecayAmount);
        }
        await Task.CompletedTask;
    }
}
