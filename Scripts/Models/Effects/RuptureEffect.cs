using System.Threading.Tasks;
using ProjectRA.Commands;
using ProjectRA.Contexts;
using ProjectRA.Keyword;

namespace ProjectRA.Models.Effects;

public sealed class RuptureEffect : StatusEffectModel
{
    public RuptureEffect()
    {
        Category = EffectCategory.DeBuff;
        HasIntensity = true;
        HasStack = true;
        DecayTiming = EffectTimingKeyword.TurnEnd;
        DecayAmount = 0;
    }

    public override async Task OnHit(DamageContext ctx)
    {
        if (Stack > 0)
        {
            await DamageCmd.Apply(Intensity, new DamageContext
            {
                Dealer = this.Owner,
                Target = ctx.Dealer,
                IsUnblockable = true,
            });
            Stack--;
        }
    }
}
