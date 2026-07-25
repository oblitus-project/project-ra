using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Combat
{
    using Core;

    /// <summary>攻击属性：斩击 / 突刺 / 打击</summary>
    public enum AttackAttribute
    {
        Slash,
        Pierce,
        Blunt
    }

    /// <summary>攻击范围类型</summary>
    public enum AttackRange
    {
        Melee,          // 近战
        Ranged,         // 远程
        MassIndividual, // 群体攻击-交锋
        MassSummation,  // 群体攻击-清算
        MassSplash      // 群体攻击-溅射
    }

    /// <summary>神秘显现类型。对应 library.md §神秘显现 的六种属性</summary>
    public enum MysteryType
    {
        Explosive,   // 爆发
        Piercing,    // 贯穿
        Corrosive,   // 分解
        Mystic,      // 神秘
        Sonic,       // 振动
        Normal      // 一般
    }

    /// <summary>装甲类型。对应 library.md 神秘显现克制表的七种装甲</summary>
    public enum ArmorType
    {
        LightArmor,     // 轻装甲
        HeavyArmor,     // 重装甲
        CompositeArmor, // 复合装甲
        SpecialArmor,   // 特殊装甲
        ElasticArmor,   // 弹力装甲
        NormalArmor,    // 一般装甲
        Structure       // 结构物
    }

    /// <summary>卡牌定位（五类）</summary>
    public enum CardType
    {
        AttackCard,   // 攻击卡：以攻击骰子为主
        GuardCard,    // 守备卡：以防御/招架/闪避骰子为主
        SupportCard,  // 辅助卡：治疗/净化/增益
        SkillCard,    // 技能卡：即时生效，不占用骰子
        AbilityCard   // 能力卡：全场生效，每场一次
    }

    /// <summary>骰子基底类型</summary>
    public enum DiceType
    {
        Offensive,  // 攻击骰子
        Defensive,  // 防御骰子
        Block,      // 招架骰子
        Evade,      // 闪避骰子
        Heal,       // 治疗骰子
        MassAttack, // 群体攻击骰子
        MassHeal    // 群体治疗骰子
    }

    /// <summary>骰子子类型修饰。反击骰子、运算骰子（锚点/衍生）等均以此表示</summary>
    public enum DiceSubType
    {
        Normal,  // 普通骰子
        Counter, // 反击骰子：受到单方面攻击时触发拼点
        Anchor,  // 锚点骰子：运算骰子的第一颗，提供基础值
        Derived  // 衍生骰子：运算骰子的剩余骰子，以上一骰子最终威力为基础运算
    }

    /// <summary>骰子威力范围（最小值~最大值）</summary>
    public struct DiceRange
    {
        public int Min;
        public int Max;

        public DiceRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>在范围内随机掷出一个值</summary>
        public int Roll(Random rng) => rng.Next(Min, Max + 1);
    }

    /// <summary>运算骰子的运算类型</summary>
    public enum OperationType
    {
        Add,      // 加算 (+)
        Subtract, // 减算 (-)
        Multiply, // 乘算 (*)
        Divide    // 除算 (/)
    }

    /// <summary>运算骰子。以基础值或上一骰子的最终威力与变动值进行运算</summary>
    public struct OperationDice
    {
        public OperationType Op;
        public DiceRange Variation;
        public bool UsePreviousAsBase;  // true: 以上一骰子最终威力为基础; false: 使用基础值

        /// <summary>对 baseValue 应用运算，返回结果</summary>
        public int Apply(int baseValue, Random rng)
        {
            int varValue = Variation.Roll(rng);
            return Op switch
            {
                OperationType.Add      => baseValue + varValue,
                OperationType.Subtract => baseValue - varValue,
                OperationType.Multiply => baseValue * varValue,
                OperationType.Divide   => varValue != 0 ? baseValue / varValue : baseValue,
                _ => baseValue
            };
        }
    }

    /// <summary>战斗中使用的骰子实例（卡牌中的一颗骰子）</summary>
    public struct DiceInstance
    {
        public DiceType Type;
        public DiceSubType SubType;
        public AttackAttribute Attribute;
        public AttackRange Range;
        public int BaseMin;         // 基础最小值
        public int BaseMax;         // 基础最大值
        public int LevelMod;        // 等级修正（如 Lv+5）
        public bool IsIndestructible; // 是否不可破坏
        public int AwValue;         // 溅射附加值（AW）
        public int HwValue;         // 治愈附加值（HW）
        public OperationDice? Operation; // 运算骰子（如有）
    }

    /// <summary>
    /// 效果触发时机。对应 affected.md 中定义的各触发条件。
    /// 用于卡片技能、被动、状态效果的时机判定。
    /// </summary>
    public enum EffectTiming
    {
        BattleStart,      // [战斗开始]
        TurnStart,        // [回合开始]
        TurnEnd,          // [回合结束]
        CombatOpen,       // [开启战斗]（仅一次）
        BattleEnd,        // [结束战斗]
        OnUse,            // [使用时]
        WhileUsing,       // [使用中]（默认）
        OnEquip,          // [装备时]
        WhileEquipped,    // [装备中]
        OnClash,          // [拼点时]
        OnClashWin,       // [拼点胜利]
        OnClashLose,      // [拼点失败]
        BeforeAttack,     // [攻击前]
        AfterAttack,      // [攻击后]
        OnHit,            // [命中时]
        OnHitUndestroyed, // [命中时 - 未破坏]
        OnHitDestroyed,   // [命中时 - 已破坏]
        OnDamageTaken,    // [受击时]
        AfterDamageTaken, // [受击后]
        OnEvade,          // [闪避成功时]
        OnEmotionGain,    // [获得情感时]
        OnConfusion,      // [陷入混乱]
        OnDown,           // [倒下时]
        OnKill,           // [击杀时]
        OnDiscard,        // [丢弃时]
        OnDraw            // [抽到时]
    }

    /// <summary>拼点解析器。处理双方骰子的拼点流程：修正 → 拼点 → 结果</summary>
    public class ClashResolver
    {
        /// <summary>拼点结果</summary>
        public struct ClashResult
        {
            public int PlayerRoll;
            public int EnemyRoll;
            public bool PlayerWins;
            public bool IsDraw;
        }

        /// <summary>
        /// 执行一次拼点判定。
        /// 流程：受理智影响的掷骰 → 等级差修正（每级 +1/3）→ 比较大小
        /// </summary>
        public static ClashResult ResolveClash(
            DiceInstance playerDice, int playerAtkLevel,
            DiceInstance enemyDice, int enemyAtkLevel,
            int playerSanity, int enemySanity, Random rng)
        {
            int pRoll = RollDiceWithSanity(playerDice, playerSanity, rng);
            int eRoll = RollDiceWithSanity(enemyDice, enemySanity, rng);

            int levelDiff = playerAtkLevel - enemyAtkLevel;
            if (levelDiff > 0)
            {
                int bonus = levelDiff / 3;
                pRoll += bonus;
            }
            else if (levelDiff < 0)
            {
                int penalty = -levelDiff / 3;
                eRoll += penalty;
            }

            return new ClashResult
            {
                PlayerRoll = pRoll,
                EnemyRoll = eRoll,
                PlayerWins = pRoll > eRoll,
                IsDraw = pRoll == eRoll
            };
        }

        /// <summary>根据理智对骰子进行加权随机掷骰</summary>
        private static int RollDiceWithSanity(DiceInstance dice, int sanity, Random rng)
        {
            var weights = SanitySystem.CalculateDiceWeights(dice.BaseMin, dice.BaseMax, sanity);
            int range = weights.EffectiveMax - weights.EffectiveMin + 1;
            float totalWeight = weights.Weights.Sum();
            float roll = (float)rng.NextDouble() * totalWeight;
            float cumulative = 0;
            for (int i = 0; i < range; i++)
            {
                cumulative += weights.Weights[i];
                if (roll <= cumulative)
                    return weights.EffectiveMin + i;
            }
            return weights.EffectiveMax;
        }
    }

    /// <summary>伤害计算器。处理神秘克制、等级压制、地形适性等加成</summary>
    public static class DamageCalculator
    {
        /// <summary>神秘显现克制表。7 种装甲 × 6 种攻击属性 → 倍率</summary>
        private static readonly Dictionary<(ArmorType, MysteryType), float> MysteryTable = new()
        {
            { (ArmorType.LightArmor,     MysteryType.Explosive),     1.5f },
            { (ArmorType.HeavyArmor,     MysteryType.Explosive),     1.0f },
            { (ArmorType.CompositeArmor, MysteryType.Explosive),     1.0f },
            { (ArmorType.SpecialArmor,   MysteryType.Explosive),     0.5f },
            { (ArmorType.ElasticArmor,   MysteryType.Explosive),     0.5f },
            { (ArmorType.NormalArmor,    MysteryType.Explosive),     1.0f },
            { (ArmorType.Structure,      MysteryType.Explosive),     0.5f },

            { (ArmorType.LightArmor,     MysteryType.Piercing),    0.5f },
            { (ArmorType.HeavyArmor,     MysteryType.Piercing),    1.5f },
            { (ArmorType.CompositeArmor, MysteryType.Piercing),    1.0f },
            { (ArmorType.SpecialArmor,   MysteryType.Piercing),    1.0f },
            { (ArmorType.ElasticArmor,   MysteryType.Piercing),    1.0f },
            { (ArmorType.NormalArmor,    MysteryType.Piercing),    1.0f },
            { (ArmorType.Structure,      MysteryType.Piercing),    0.5f },

            { (ArmorType.LightArmor,     MysteryType.Corrosive), 0.5f },
            { (ArmorType.HeavyArmor,     MysteryType.Corrosive), 1.5f },
            { (ArmorType.CompositeArmor, MysteryType.Corrosive), 1.0f / 1.5f },
            { (ArmorType.SpecialArmor,   MysteryType.Corrosive), 1.0f },
            { (ArmorType.ElasticArmor,   MysteryType.Corrosive), 1.0f },
            { (ArmorType.NormalArmor,    MysteryType.Corrosive), 1.0f },
            { (ArmorType.Structure,      MysteryType.Corrosive), 0.5f },

            { (ArmorType.LightArmor,     MysteryType.Mystic),    1.0f },
            { (ArmorType.HeavyArmor,     MysteryType.Mystic),    0.5f },
            { (ArmorType.CompositeArmor, MysteryType.Mystic),    0.5f },
            { (ArmorType.SpecialArmor,   MysteryType.Mystic),    1.5f },
            { (ArmorType.ElasticArmor,   MysteryType.Mystic),    1.0f },
            { (ArmorType.NormalArmor,    MysteryType.Mystic),    1.0f },
            { (ArmorType.Structure,      MysteryType.Mystic),    0.5f },

            { (ArmorType.LightArmor,     MysteryType.Sonic), 1.0f },
            { (ArmorType.HeavyArmor,     MysteryType.Sonic), 0.5f },
            { (ArmorType.CompositeArmor, MysteryType.Sonic), 0.5f },
            { (ArmorType.SpecialArmor,   MysteryType.Sonic), 1.5f },
            { (ArmorType.ElasticArmor,   MysteryType.Sonic), 1.0f / 1.5f },
            { (ArmorType.NormalArmor,    MysteryType.Sonic), 1.0f },
            { (ArmorType.Structure,      MysteryType.Sonic), 0.5f },

            { (ArmorType.LightArmor,     MysteryType.Normal),    1.0f },
            { (ArmorType.HeavyArmor,     MysteryType.Normal),    1.0f },
            { (ArmorType.CompositeArmor, MysteryType.Normal),    1.0f },
            { (ArmorType.SpecialArmor,   MysteryType.Normal),    1.0f },
            { (ArmorType.ElasticArmor,   MysteryType.Normal),    1.0f },
            { (ArmorType.NormalArmor,    MysteryType.Normal),    1.0f },
            { (ArmorType.Structure,      MysteryType.Normal),    0.5f },
        };

        /// <summary>查表获取神秘显现倍率（Resist=0.5, Normal=1.0, Efficient=1.5, Weak=2.0）</summary>
        public static float GetMysteryMultiplier(ArmorType armor, MysteryType mysteryType)
        {
            if (MysteryTable.TryGetValue((armor, mysteryType), out float mult))
                return mult;
            return 1.0f;
        }

        /// <summary>
        /// 等级压制计算。
        /// 攻击时：攻等每高1级，伤害增加 [等级差/(等级差+10)]%
        /// 受击时：防等每高1级，受伤降低 [等级差/(等级差+10)]%
        /// </summary>
        public static float CalcLevelSuppression(int atkLevel, int defLevel, bool isAttacking)
        {
            int diff = Math.Abs(atkLevel - defLevel);
            float rate = diff / (float)(diff + 10);
            if (atkLevel > defLevel)
                return isAttacking ? 1f + rate : 1f - rate;
            else if (atkLevel < defLevel)
                return isAttacking ? 1f - rate : 1f + rate;
            return 1f;
        }
    }

    /// <summary>守备骰子解析器。处理不同类型守备骰子 vs 攻击骰子的拼点结果</summary>
    public static class GuardDiceResolver
    {
        /// <summary>守备骰子的拼点结果</summary>
        public enum GuardResult
        {
            ShieldGained,         // 获得护盾（防御骰子 vs 攻击骰子）
            CounterConfusionDmg,  // 反震混乱伤害（招架骰子 vs 攻击骰子/招架骰子）
            EvadeSuccess,         // 闪避成功（闪避骰子 vs 攻击骰子）
            Consumed,             // 被消耗
            TriggerCounter        // 触发反击（反击骰子）
        }

        /// <summary>根据守备骰子和攻击骰子的类型返回拼点结果。反击子类型的判定在外部处理</summary>
        public static GuardResult Resolve(DiceType guardDice, DiceType attackerDice)
        {
            return (guardDice, attackerDice) switch
            {
                (DiceType.Defensive, DiceType.Offensive) => GuardResult.ShieldGained,
                (DiceType.Defensive, _)                  => GuardResult.Consumed,

                (DiceType.Block, DiceType.Offensive)     => GuardResult.CounterConfusionDmg,
                (DiceType.Block, DiceType.Defensive)     => GuardResult.Consumed,
                (DiceType.Block, DiceType.Block)         => GuardResult.CounterConfusionDmg,
                (DiceType.Block, DiceType.Evade)         => GuardResult.Consumed,

                (DiceType.Evade, DiceType.Offensive)     => GuardResult.EvadeSuccess,
                (DiceType.Evade, DiceType.Defensive)     => GuardResult.Consumed,
                (DiceType.Evade, DiceType.Block)         => GuardResult.Consumed,
                (DiceType.Evade, DiceType.Evade)         => GuardResult.Consumed,

                _ => GuardResult.Consumed
            };
        }
    }

    /// <summary>地形适性数据行。对应 library.md §地形适性 表格</summary>
    public struct TerrainAdaptationRow
    {
        public char Grade;              // D / C / B / A / S / R
        public float CoverTransferRate; // 掩体转移伤害比例
        public float SelfDamageRate;    // 自身造成伤害比例
    }

    /// <summary>地形适性表</summary>
    public static class TerrainAdaptationTable
    {
        private static readonly Dictionary<char, TerrainAdaptationRow> Rows = new()
        {
            { 'D', new TerrainAdaptationRow { Grade = 'D', CoverTransferRate = 0.00f, SelfDamageRate = -0.20f } },
            { 'C', new TerrainAdaptationRow { Grade = 'C', CoverTransferRate = 0.10f, SelfDamageRate = -0.10f } },
            { 'B', new TerrainAdaptationRow { Grade = 'B', CoverTransferRate = 0.20f, SelfDamageRate = 0.00f } },
            { 'A', new TerrainAdaptationRow { Grade = 'A', CoverTransferRate = 0.30f, SelfDamageRate = 0.10f } },
            { 'S', new TerrainAdaptationRow { Grade = 'S', CoverTransferRate = 0.40f, SelfDamageRate = 0.20f } },
            { 'R', new TerrainAdaptationRow { Grade = 'R', CoverTransferRate = 0.50f, SelfDamageRate = 0.30f } },
        };

        public static TerrainAdaptationRow? Get(char grade) =>
            Rows.TryGetValue(grade, out var row) ? row : null;
    }

    /// <summary>加成层枚举。对应 library.md §加成 的六类加成</summary>
    public enum BonusLayer
    {
        Terrain_LevelSuppression,      // 一类：地形适性、等级压制
        MysteryAffinity,               // 二类：神秘克制
        DamageResistance,              // 三类：伤害抗性
        DamageRate_TakenRate_Proficiency, // 四类：伤害率、承伤率、熟练度
        CritHit,                   // 五类：暴击
        DealDamage_TakeDamage          // 六类：造成伤害、受到伤害
    }
}
