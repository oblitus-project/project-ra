using System.Threading.Tasks;
using ProjectRA.Combat;
using ProjectRA.Contexts;
using ProjectRA.Entities;
using ProjectRA.Hooks;
using ProjectRA.Models;

namespace ProjectRA.Commands;

public static class DamageCmd
{
    // Direct unblockable damage (for effects: Burn, Poison, Rupture, etc.)
    public static async Task Apply(decimal amount, DamageContext ctx)
    {
        ctx.BaseDamage = amount;
        ctx.FinalDamage = amount;
        ctx.Target.Hp -= (int)amount;
        await Hook.OnHit(ctx.Target, ctx);
        await Hook.OnDamageTaken(ctx.Target, ctx);
    }

    public static DamageBuilder Calculate() => new();

    public class DamageBuilder
    {
        private Creature _source;
        private Creature _target;
        private DiceInstance _dice;
        private int _finalPower;
        private MysteryType _mysteryType;
        private char _terrainGrade = 'B';
        private decimal _cardIntrinsicMod; // Layer 6: e.g. 0.50m = +50%
        private bool _isFromClash;

        public DamageBuilder FromSource(Creature source, DiceInstance dice, int finalPower, MysteryType mysteryType)
        {
            _source = source;
            _dice = dice;
            _finalPower = finalPower;
            _mysteryType = mysteryType;
            return this;
        }

        public DamageBuilder ToTarget(Creature target)
        {
            _target = target;
            return this;
        }

        public DamageBuilder WithTerrainGrade(char grade)
        {
            _terrainGrade = grade;
            return this;
        }

        public DamageBuilder WithCardIntrinsicMod(decimal mod)
        {
            _cardIntrinsicMod = mod;
            return this;
        }

        public DamageBuilder FromClash(bool value)
        {
            _isFromClash = value;
            return this;
        }

        public async Task<DamageResult> ExecuteAsync()
        {
            decimal baseDamage = _finalPower;

            var ctx = new DamageContext
            {
                Dealer = _source,
                Target = _target,
                BaseDamage = baseDamage,
                MysteryType = _mysteryType,
                TargetArmor = _target.ArmorType,
                Attribute = _dice.Attribute,
                IsFromClash = _isFromClash,
            };

            // Layer 1: 地形适性 + 等级压制
            decimal layer1 = 1m;
            var terrain = TerrainAdaptationTable.Get(_terrainGrade);
            if (terrain.HasValue)
                layer1 += terrain.Value.SelfDamageRate;
            float levelSup = DamageCalculator.CalcLevelSuppression(
                _source.AtkLevel, _target.DefLevel, isAttacking: true);
            layer1 += levelSup - 1f;

            // Layer 2: 神秘克制
            decimal layer2 = DamageCalculator.GetMysteryMultiplier(
                _target.ArmorType, _mysteryType);

            // Layer 3-5: 效果遍历
            decimal layer3 = 1m;
            decimal layer4 = 1m;
            decimal layer5 = 1m;

            ctx.CurrentRole = ModifierRole.Source;
            foreach (var effect in _source.ActiveEffects)
            {
                layer3 += effect.ModifyBonusLayer(BonusLayer.DamageResistance, ctx);
                layer4 += effect.ModifyBonusLayer(BonusLayer.DamageRate_TakenRate_Proficiency, ctx);
                layer5 += effect.ModifyBonusLayer(BonusLayer.CritHit, ctx);
            }

            ctx.CurrentRole = ModifierRole.Target;
            foreach (var effect in _target.ActiveEffects)
            {
                layer3 += effect.ModifyBonusLayer(BonusLayer.DamageResistance, ctx);
                layer4 += effect.ModifyBonusLayer(BonusLayer.DamageRate_TakenRate_Proficiency, ctx);
                layer5 += effect.ModifyBonusLayer(BonusLayer.CritHit, ctx);
            }

            // Layer 6: 卡牌词条
            decimal layer6 = 1m + _cardIntrinsicMod;

            decimal finalDamage = baseDamage
                * layer1 * layer2 * layer3 * layer4 * layer5 * layer6;

            ctx.FinalDamage = finalDamage;

            await Hook.OnHit(_target, ctx);
            await Hook.OnDamageTaken(_target, ctx);

            return new DamageResult
            {
                AttackDamage = finalDamage,
                ConfusionDamage = finalDamage,
            };
        }
    }
}
