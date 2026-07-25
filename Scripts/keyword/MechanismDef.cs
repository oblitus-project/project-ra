using System;

namespace ProjectRA.Keyword
{
    /// <summary>基础状态。对应 mechanism.md §基础状态</summary>
    public enum BasicState
    {
        Killed,          // 阵亡
        Defeated,        // 倒下
        Retreat,         // 撤退
        AttackDamage,    // 攻击伤害
        StaggerDamage,   // 混乱伤害
        Staggered,       // 陷入混乱
        Immobilized,     // 无法行动
        CantDraw,        // 无法抽卡
        CantEquip,       // 无法装备
        Untargetable,    // 不可选中
        Unstoppable,     // 无法中止
        Inescapable,     // 无法逃脱
        RandomAttack     // 无差别攻击
    }

    /// <summary>卡牌关键字。对应 mechanism.md §卡牌操作</summary>
    public enum CardKeyword
    {
        Upgrade,     // 升级
        Innate,      // 固有
        Discard,     // 丢弃
        Retain,      // 保留
        Exhaust,     // 消耗
        Void,        // 虚无
        Oblivion,    // 遗忘
        Unplayable,  // 不能被打出
        Eternal,     // 永恒
        Transform,   // 变化
        Staunch      // 坚定
    }

    /// <summary>战斗机制。对应 mechanism.md §战斗机制</summary>
    public enum BattleMechanism
    {
        Fixed,              // 固定
        FixedTarget,        // 固定目标
        Interrupt,          // 打断
        Amplify,            // 增幅
        CritHit,            // 暴击
        IndestructibleDice, // 不可破坏的骰子
        Unclashable,        // 不可拼点
        IgnoreCover,        // 无视掩体
        SeverDice,          // 截除骰子
        EraseDice,          // 抹杀骰子
        StrongDeflect,      // 强效偏转
        WildShot,           // 广域乱射
        TremorTransformation,// 振幅转换
        TremorBurst         // 震颤引爆
    }

    /// <summary>防御机制。对应 mechanism.md §防御机制</summary>
    public enum DefenseMechanism
    {
        Cover,             // 援护
        CoverParry,        // 援护招架
        CoverCounter,      // 援护反击
        Shield,            // 护盾
        SettlementCounter, // 清算反击
        IndestructibleDef, // 坚不可摧
        Unbreakable,       // 牢不可破
        ShrugOff           // 耸肩无视
    }

    /// <summary>资源机制。对应 mechanism.md §资源与抽卡</summary>
    public enum ResourceMechanism
    {
        DrawIncrease, // 抽卡增加
        DrawDecrease, // 抽卡减少
        Phone         // 手机
    }

    /// <summary>特殊操作。对应 mechanism.md §特殊操作</summary>
    public enum SpecialOperation
    {
        RemoveEmotion, // 移除情绪
        FixEmotion,    // 固定情绪
        Purify,        // 净化
        Dispel,        // 驱散
        Forbid,        // 不许
        Duel,          // 对决
        Obey,          // 遵命
        EISErosion,    // EIS侵蚀状态
        FreeAttack,    // 免费攻击
        FreeSkill,     // 免费技能
        FreeAbility    // 免费能力
    }

    /// <summary>标记类型。对应 mechanism.md §标记系统</summary>
    public enum MarkType
    {
        SenseiMark,   // 伤害老师的标记
        QiangMark,    // 强哥标记
        WeaknessMark, // 弱点标记
        WangMark      // 王桑的标记
    }

    /// <summary>
    /// 机制条目。保存机制的 id 和对应的本地化 key。
    /// Name / Description 通过 LocalizationManager 动态获取当前语言文本。
    /// </summary>
    public class MechanismTag
    {
        public string Id { get; }
        public string NameKey { get; }
        public string DescKey { get; }
        public string Name => LocalizationManager.Get(NameKey);
        public string Description => LocalizationManager.Get(DescKey);

        public MechanismTag(string id, string nameKey, string descKey)
        {
            Id = id;
            NameKey = nameKey;
            DescKey = descKey;
        }
    }

    /// <summary>
    /// 机制数据库。集中管理所有基础状态、卡牌关键字、战斗/防御机制、标记等。
    /// 数据源自 doc/build/keyword/mechanism.md
    /// </summary>
    public static class MechanismDB
    {
        /// <summary>13 种基础状态</summary>
        public static readonly MechanismTag[] BasicStates =
        {
            new("killed", "basic_state_killed", "basic_state_killed_desc"),
            new("downed", "basic_state_downed", "basic_state_downed_desc"),
            new("retreat", "basic_state_retreat", "basic_state_retreat_desc"),
            new("attack_damage", "basic_state_attack_damage", "basic_state_attack_damage_desc"),
            new("confusion_damage", "basic_state_confusion_damage", "basic_state_confusion_damage_desc"),
            new("confused", "basic_state_confused", "basic_state_confused_desc"),
            new("immobilized", "basic_state_immobilized", "basic_state_immobilized_desc"),
            new("cant_draw", "basic_state_cant_draw", "basic_state_cant_draw_desc"),
            new("cant_equip", "basic_state_cant_equip", "basic_state_cant_equip_desc"),
            new("untargetable", "basic_state_untargetable", "basic_state_untargetable_desc"),
            new("unstoppable", "basic_state_unstoppable", "basic_state_unstoppable_desc"),
            new("inescapable", "basic_state_inescapable", "basic_state_inescapable_desc"),
            new("random_attack", "basic_state_random_attack", "basic_state_random_attack_desc"),
        };

        /// <summary>11 种卡牌关键字</summary>
        public static readonly MechanismTag[] CardOperations =
        {
            new("upgrade", "card_op_upgrade", "card_op_upgrade_desc"),
            new("innate", "card_op_innate", "card_op_innate_desc"),
            new("discard", "card_op_discard", "card_op_discard_desc"),
            new("retain", "card_op_retain", "card_op_retain_desc"),
            new("exhaust", "card_op_exhaust", "card_op_exhaust_desc"),
            new("void", "card_op_void", "card_op_void_desc"),
            new("oblivion", "card_op_oblivion", "card_op_oblivion_desc"),
            new("unplayable", "card_op_unplayable", "card_op_unplayable_desc"),
            new("eternal", "card_op_eternal", "card_op_eternal_desc"),
            new("transform", "card_op_transform", "card_op_transform_desc"),
            new("staunch", "card_op_staunch", "card_op_staunch_desc"),
        };

        /// <summary>14 种战斗机制</summary>
        public static readonly MechanismTag[] BattleMechanisms =
        {
            new("fixed", "battle_mech_fixed", "battle_mech_fixed_desc"),
            new("fixed_target", "battle_mech_fixed_target", "battle_mech_fixed_target_desc"),
            new("interrupt", "battle_mech_interrupt", "battle_mech_interrupt_desc"),
            new("amplify", "battle_mech_amplify", "battle_mech_amplify_desc"),
            new("critical_hit", "battle_mech_critical_hit", "battle_mech_critical_hit_desc"),
            new("indestructible_dice", "battle_mech_indestructible_dice", "battle_mech_indestructible_dice_desc"),
            new("unclashable", "battle_mech_unclashable", "battle_mech_unclashable_desc"),
            new("ignore_cover", "battle_mech_ignore_cover", "battle_mech_ignore_cover_desc"),
            new("sever_dice", "battle_mech_sever_dice", "battle_mech_sever_dice_desc"),
            new("erase_dice", "battle_mech_erase_dice", "battle_mech_erase_dice_desc"),
            new("strong_deflect", "battle_mech_strong_deflect", "battle_mech_strong_deflect_desc"),
            new("wild_shot", "battle_mech_wild_shot", "battle_mech_wild_shot_desc"),
            new("tremor_transformation", "battle_mech_tremor_transformation", "battle_mech_tremor_transformation_desc"),
            new("tremor_burst", "battle_mech_tremor_burst", "battle_mech_tremor_burst_desc"),
        };

        /// <summary>8 种防御机制</summary>
        public static readonly MechanismTag[] DefenseMechanisms =
        {
            new("cover", "def_mech_cover", "def_mech_cover_desc"),
            new("cover_parry", "def_mech_cover_parry", "def_mech_cover_parry_desc"),
            new("cover_counter", "def_mech_cover_counter", "def_mech_cover_counter_desc"),
            new("shield", "def_mech_shield", "def_mech_shield_desc"),
            new("settlement_counter", "def_mech_settlement_counter", "def_mech_settlement_counter_desc"),
            new("indestructible_def", "def_mech_indestructible_def", "def_mech_indestructible_def_desc"),
            new("unbreakable", "def_mech_unbreakable", "def_mech_unbreakable_desc"),
            new("shrug_off", "def_mech_shrug_off", "def_mech_shrug_off_desc"),
        };

        /// <summary>3 种资源机制</summary>
        public static readonly MechanismTag[] Resources =
        {
            new("draw_increase", "resource_draw_increase", "resource_draw_increase_desc"),
            new("draw_decrease", "resource_draw_decrease", "resource_draw_decrease_desc"),
            new("phone", "resource_phone", "resource_phone_desc"),
        };

        /// <summary>11 种特殊操作</summary>
        public static readonly MechanismTag[] SpecialOperations =
        {
            new("remove_emotion", "spec_op_remove_emotion", "spec_op_remove_emotion_desc"),
            new("fix_emotion", "spec_op_fix_emotion", "spec_op_fix_emotion_desc"),
            new("purify", "spec_op_purify", "spec_op_purify_desc"),
            new("dispel", "spec_op_dispel", "spec_op_dispel_desc"),
            new("forbid", "spec_op_forbid", "spec_op_forbid_desc"),
            new("duel", "spec_op_duel", "spec_op_duel_desc"),
            new("obey", "spec_op_obey", "spec_op_obey_desc"),
            new("eis_erosion", "spec_op_eis_erosion", "spec_op_eis_erosion_desc"),
            new("free_attack", "spec_op_free_attack", "spec_op_free_attack_desc"),
            new("free_skill", "spec_op_free_skill", "spec_op_free_skill_desc"),
            new("free_ability", "spec_op_free_ability", "spec_op_free_ability_desc"),
        };

        /// <summary>4 种标记</summary>
        public static readonly MechanismTag[] Marks =
        {
            new("sensei_mark", "mark_sensei", "mark_sensei_desc"),
            new("qiang_mark", "mark_qiang", "mark_qiang_desc"),
            new("weakness_mark", "mark_weakness", "mark_weakness_desc"),
            new("wang_mark", "mark_wang", "mark_wang_desc"),
        };
    }
}
