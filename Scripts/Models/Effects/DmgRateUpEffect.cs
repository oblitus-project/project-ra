using ProjectRA.Combat;
using ProjectRA.Contexts;
using ProjectRA.Keyword;

namespace ProjectRA.Models.Effects;

public sealed class DmgRateUpEffect : StatusEffectModel
{
    public DmgRateUpEffect()
    {
        Category = Keyword.EffectCategory.Buff;
        DecayTiming = Keyword.EffectTimingKeyword.TurnEnd;
        DecayAmount = 0;
        HasStack = true;
    }

    public override decimal ModifyBonusLayer(BonusLayer layer, DamageContext ctx)
    {
        if (layer == BonusLayer.DamageRate_TakenRate_Proficiency
            && ctx.CurrentRole == ModifierRole.Source
            && ctx.Dealer == this.Owner)
        {
            return Stack * 0.05m;
        }
        return 0m;
    }
}
