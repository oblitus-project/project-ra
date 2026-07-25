using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectRA.Combat;
using ProjectRA.Contexts;
using ProjectRA.Entities;
using ProjectRA.Models;

namespace ProjectRA.Hooks;

public static class Hook
{
    private static IEnumerable<StatusEffectModel> IterateEffects(Creature creature)
    {
        return creature.ActiveEffects;
    }

    // ============ 战斗生命周期 ============

    public static async Task OnBattleStart(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnBattleStart();
    }

    public static async Task OnTurnStart(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnTurnStart();
    }

    public static async Task OnTurnEnd(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnTurnEnd();
    }

    public static async Task OnBattleEnd(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnBattleEnd();
    }

    // ============ 情感系统 ============

    public static async Task OnEmotionGain(Creature creature, int amount)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnEmotionGain(amount);
    }

    // ============ 速度骰 ============

    public static async Task OnSpeedDiceRolled(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnSpeedDiceRolled();
    }

    // ============ 拼点 ============

    public static async Task BeforeClash(ClashContext ctx)
    {
        foreach (var effect in IterateEffects(ctx.Attacker))
            await effect.BeforeClash(ctx);
        foreach (var effect in IterateEffects(ctx.Defender))
            await effect.BeforeClash(ctx);
    }

    public static async Task OnClashWin(Creature winner, ClashContext ctx)
    {
        foreach (var effect in IterateEffects(winner))
            await effect.OnClashWin(ctx);
    }

    public static async Task OnClashLose(Creature loser, ClashContext ctx)
    {
        foreach (var effect in IterateEffects(loser))
            await effect.OnClashLose(ctx);
    }

    // ============ 骰子威力 ============

    public static int ModifyDicePower(Creature creature, int power, DiceContext ctx)
    {
        int result = power;
        foreach (var effect in IterateEffects(creature))
            result = effect.ModifyDicePower(result, ctx);
        return result;
    }

    // ============ 受击/击杀 ============

    public static async Task OnHit(Creature target, DamageContext ctx)
    {
        foreach (var effect in IterateEffects(target))
            await effect.OnHit(ctx);
    }

    public static async Task OnDamageTaken(Creature target, DamageContext ctx)
    {
        foreach (var effect in IterateEffects(target))
            await effect.OnDamageTaken(ctx);
    }

    public static async Task OnConfusion(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnConfusion();
    }

    public static async Task OnKill(Creature killer, Creature target)
    {
        foreach (var effect in IterateEffects(killer))
            await effect.OnKill(target);
    }

    public static async Task OnDown(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnDown();
    }

    // ============ 卡牌事件 ============

    public static async Task OnCardPlayed(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnCardPlayed();
    }

    public static async Task OnCardDrawn(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnCardDrawn();
    }

    public static async Task OnCardDiscarded(Creature creature)
    {
        foreach (var effect in IterateEffects(creature))
            await effect.OnCardDiscarded();
    }
}
