using System;
using System.Threading.Tasks;
using ProjectRA.Commands;
using ProjectRA.Contexts;
using ProjectRA.Keyword;

namespace ProjectRA.Models.Effects;

public sealed class TremorEffect : StatusEffectModel
{
    public TremorEffect()
    {
        Category = EffectCategory.DeBuff;
        HasStack = true;
        DecayTiming = EffectTimingKeyword.TurnEnd;
        DecayAmount = 0;
    }

    public async Task Burst()
    {
        if (Stack <= 0) return;

        await DamageCmd.Apply(Stack, new DamageContext
        {
            Dealer = this.Owner,
            Target = this.Owner,
            IsUnblockable = true,
            IsConfusionDamage = true,
        });

        Stack = (int)Math.Floor(Stack * 2f / 3f);
    }
}
