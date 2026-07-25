using ProjectRA.Combat;
using ProjectRA.Contexts;
using ProjectRA.Keyword;

namespace ProjectRA.Models.Effects;

public sealed class WeakEffect : StatusEffectModel
{
    public WeakEffect()
    {
        Category = Keyword.EffectCategory.DeBuff;
        DecayTiming = Keyword.EffectTimingKeyword.TurnEnd;
        DecayAmount = 1;
        HasStack = true;
    }

    public override decimal ModifyBonusLayer(BonusLayer layer, DamageContext ctx)
    {
        if (layer == BonusLayer.DamageRate_TakenRate_Proficiency
            && ctx.CurrentRole == ModifierRole.Source
            && ctx.Dealer == this.Owner)
        {
            return -0.25m;
        }
        return 0m;
    }
}
