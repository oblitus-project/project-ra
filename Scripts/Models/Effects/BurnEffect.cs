using System.Threading.Tasks;
using ProjectRA.Commands;
using ProjectRA.Contexts;
using ProjectRA.Keyword;

namespace ProjectRA.Models.Effects;

public sealed class BurnEffect : StatusEffectModel
{
    public BurnEffect()
    {
        Category = EffectCategory.DeBuff;
        DecayTiming = EffectTimingKeyword.TurnEnd;
        DecayAmount = 0;
        HasStack = true;
        HasIntensity = false;
    }

    public override async Task OnTurnEnd()
    {
        if (Stack > 0)
        {
            await DamageCmd.Apply(Stack, new DamageContext
            {
                Dealer = this.Owner,
                Target = this.Owner,
                IsUnblockable = true,
            });
        }
    }
}
