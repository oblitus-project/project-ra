using System.Threading.Tasks;
using ProjectRA.Contexts;
using ProjectRA.Keyword;

namespace ProjectRA.Models.Effects;

public sealed class InvigorateEffect : StatusEffectModel
{
    public InvigorateEffect()
    {
        Category = EffectCategory.Buff;
        HasStack = true;
        MaxStack = 10;
        DecayTiming = Keyword.EffectTimingKeyword.TurnEnd;
        DecayAmount = 0;
    }

    public override async Task OnClashWin(ClashContext ctx)
    {
        if (ctx.Attacker == this.Owner)
            AddStack(1);
        await Task.CompletedTask;
    }
}
