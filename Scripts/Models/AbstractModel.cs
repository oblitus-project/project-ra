using System;
using System.Threading.Tasks;
using ProjectRA.Combat;
using ProjectRA.Contexts;
using ProjectRA.Entities;

namespace ProjectRA.Models;

public abstract class AbstractModel
{
    public ModelId Id { get; }
    public bool IsMutable { get; private set; }
    public bool IsCanonical => !IsMutable;

    protected AbstractModel()
    {
        Id = ModelDb.GenerateId(GetType());
    }

    public void AssertMutable()
    {
        if (!IsMutable)
            throw new InvalidOperationException($"Model {Id} is canonical. Call ToMutable() first.");
    }

    public AbstractModel ToMutable()
    {
        var clone = (AbstractModel)MemberwiseClone();
        clone.IsMutable = true;
        clone.AfterCloned();
        return clone;
    }

    protected virtual void AfterCloned() { }

    // ============ 战斗生命周期 ============

    public virtual Task OnBattleStart() => Task.CompletedTask;
    public virtual Task OnTurnStart() => Task.CompletedTask;
    public virtual Task OnTurnEnd() => Task.CompletedTask;
    public virtual Task OnBattleEnd() => Task.CompletedTask;

    // ============ 情感系统 ============

    public virtual Task OnEmotionGain(int amount) => Task.CompletedTask;
    public virtual Task OnEmotionLevelUp(int newLevel) => Task.CompletedTask;

    // ============ 速度骰 ============

    public virtual Task OnSpeedDiceRolled() => Task.CompletedTask;

    // ============ 拼点 ============

    public virtual Task BeforeClash(ClashContext ctx) => Task.CompletedTask;
    public virtual Task OnClashWin(ClashContext ctx) => Task.CompletedTask;
    public virtual Task OnClashLose(ClashContext ctx) => Task.CompletedTask;

    // ============ 伤害管道 (6层加成) ============
    // 返回偏移量: 0 = 无修正, 0.25 = +25%, -0.25 = -25%
    // 层间乘算, 层内加算
    public virtual decimal ModifyBonusLayer(BonusLayer layer, DamageContext ctx) => 0m;

    // ============ 骰子威力 ============

    public virtual int ModifyDicePower(int power, DiceContext ctx) => power;

    // ============ 受击/击杀 ============

    public virtual Task OnHit(DamageContext ctx) => Task.CompletedTask;
    public virtual Task OnDamageTaken(DamageContext ctx) => Task.CompletedTask;
    public virtual Task OnConfusion() => Task.CompletedTask;
    public virtual Task OnKill(Creature target) => Task.CompletedTask;
    public virtual Task OnDown() => Task.CompletedTask;

    // ============ 卡牌事件 ============

    public virtual Task OnCardPlayed() => Task.CompletedTask;
    public virtual Task OnCardDrawn() => Task.CompletedTask;
    public virtual Task OnCardDiscarded() => Task.CompletedTask;
}
