using System;

namespace ProjectRA.Keyword
{
    /// <summary>
    /// 效果类别（可组合的 Flags 枚举）。
    /// Buff/DeBuff 标记，Special/Indestructible 决定能否被净化/驱散，CC(CrowdControl) 表示限制行动类。
    /// </summary>
    [Flags]
    public enum EffectCategory
    {
        Buff = 1 << 0,              // 正面效果（可被驱散移除）
        DeBuff = 1 << 1,            // 负面效果（可被净化移除）
        Special = 1 << 2,           // 特殊效果（不可被净化/驱散移除）
        CC = 1 << 3,                // 群控效果（CrowdControl）
        Indestructible = 1 << 4     // 不可摧毁（#pRA/类型/不可摧毁）
    }

    /// <summary>效果的基础衰减/触发时机</summary>
    public enum EffectTimingKeyword
    {
        TurnEnd,    // [回合结束] 触发或衰减
        TurnStart,  // [回合开始] 触发
        OnDamaged,  // [受击时] 触发
        OnHit,      // [命中时] 触发
        OnTrigger,  // [触发时] 触发（如震颤）
        OnCritHit,  // [暴击命中时] 触发
        Immediate   // 立即生效
    }

    /// <summary>
    /// 状态效果数据定义。对应 effect.md 中每个效果的元数据。
    /// Name / Description 通过 LocalizationManager 获取当前语言文本。
    /// </summary>
    public struct StatusEffectData
    {
        public string Id;                     // 效果唯一标识
        public string NameKey;                // 名称本地化 key
        public string DescriptionKey;         // 描述本地化 key
        public EffectCategory Category;       // 效果类别
        public int MaxStack;                  // 最大层数（默认无上限）
        public EffectTimingKeyword DecayTiming; // 衰减时机
        public int DecayAmount;               // 每回合衰减量
        public bool HasIntensity;             // 是否有强度（P）参数，如 呼吸法 P/X, 破裂 P/X
        public bool HasStack;                 // 是否有层数（X）参数

        public string Name => LocalizationManager.Get(NameKey);
        public string Description => LocalizationManager.Get(DescriptionKey);

        public StatusEffectData(string id, string nameKey, string descriptionKey,
            EffectCategory category, int maxStack = int.MaxValue,
            EffectTimingKeyword decayTiming = EffectTimingKeyword.TurnEnd,
            int decayAmount = 1, bool hasIntensity = false, bool hasStack = true)
        {
            Id = id;
            NameKey = nameKey;
            DescriptionKey = descriptionKey;
            Category = category;
            MaxStack = maxStack;
            DecayTiming = decayTiming;
            DecayAmount = decayAmount;
            HasIntensity = hasIntensity;
            HasStack = hasStack;
        }
    }

    /// <summary>战斗中生效中的效果实例（带有层数/强度/剩余回合数）</summary>
    public struct ActiveEffect
    {
        public string EffectId;  // 对应 StatusEffectData.Id
        public int Stack;        // 当前层数 X
        public int Intensity;    // 当前强度 P
        public int RemainingTurns; // 剩余回合数
    }

    /// <summary>
    /// 效果数据库。集中管理所有状态效果的定义。
    /// 数据源自 doc/build/keyword/effect.md
    /// </summary>
    public static class EffectDB
    {
        /// <summary>30 种正面效果</summary>
        public static readonly StatusEffectData[] PositiveEffects =
        {
            new("dmg_rate_up", "effect_dmg_rate_up", "effect_dmg_rate_up_desc", EffectCategory.Buff),
            new("dmg_up", "effect_dmg_up", "effect_dmg_up_desc", EffectCategory.Buff),
            new("dmg_slash_up", "effect_dmg_slash_up", "effect_dmg_slash_up_desc", EffectCategory.Buff),
            new("dmg_pierce_up", "effect_dmg_pierce_up", "effect_dmg_pierce_up_desc", EffectCategory.Buff),
            new("dmg_blunt_up", "effect_dmg_blunt_up", "effect_dmg_blunt_up_desc", EffectCategory.Buff),
            new("confusion_dmg_up", "effect_confusion_dmg_up", "effect_confusion_dmg_up_desc", EffectCategory.Buff),
            new("atk_dmg_up", "effect_atk_dmg_up", "effect_atk_dmg_up_desc", EffectCategory.Buff),
            new("res_down", "effect_res_down", "effect_res_down_desc", EffectCategory.Buff),
            new("atk_res_down", "effect_atk_res_down", "effect_atk_res_down_desc", EffectCategory.Buff),
            new("conf_res_down", "effect_conf_res_down", "effect_conf_res_down_desc", EffectCategory.Buff),
            new("heal_rate_up", "effect_heal_rate_up", "effect_heal_rate_up_desc", EffectCategory.Buff),
            new("conf_dmg_taken_down", "effect_conf_dmg_taken_down", "effect_conf_dmg_taken_down_desc", EffectCategory.Buff),
            new("atk_dmg_taken_down", "effect_atk_dmg_taken_down", "effect_atk_dmg_taken_down_desc", EffectCategory.Buff),
            new("dmg_taken_down", "effect_dmg_taken_down", "effect_dmg_taken_down_desc", EffectCategory.Buff),
            new("specialty_up", "effect_specialty_up", "effect_specialty_up_desc", EffectCategory.Buff),
            new("strong", "effect_strong", "effect_strong_desc", EffectCategory.Buff),
            new("endure", "effect_endure", "effect_endure_desc", EffectCategory.Buff),
            new("dice_power_up", "effect_dice_power_up", "effect_dice_power_up_desc", EffectCategory.Buff),
            new("clash_power_up", "effect_clash_power_up", "effect_clash_power_up_desc", EffectCategory.Buff),
            new("atk_level_up", "effect_atk_level_up", "effect_atk_level_up_desc", EffectCategory.Buff),
            new("def_level_up", "effect_def_level_up", "effect_def_level_up_desc", EffectCategory.Buff),
            new("dmg_received_down", "effect_dmg_received_down", "effect_dmg_received_down_desc", EffectCategory.Buff),
            new("conf_dmg_received_down", "effect_conf_dmg_received_down", "effect_conf_dmg_received_down_desc", EffectCategory.Buff),
            new("atk_dmg_received_down", "effect_atk_dmg_received_down", "effect_atk_dmg_received_down_desc", EffectCategory.Buff),
            new("invigorate", "effect_invigorate", "effect_invigorate_desc", EffectCategory.Buff, maxStack: 10),
            new("guard", "effect_guard", "effect_guard_desc", EffectCategory.Buff, maxStack: 10),
            new("swift", "effect_swift", "effect_swift_desc", EffectCategory.Buff),
            new("breathing", "effect_breathing", "effect_breathing_desc", EffectCategory.Buff, hasIntensity: true),
            new("artifact", "effect_artifact", "effect_artifact_desc", EffectCategory.Buff),
            new("regen", "effect_regen", "effect_regen_desc", EffectCategory.Buff),
        };

        /// <summary>35 种负面效果</summary>
        public static readonly StatusEffectData[] NegativeEffects =
        {
            new("dmg_rate_down", "effect_dmg_rate_down", "effect_dmg_rate_down_desc", EffectCategory.DeBuff),
            new("dmg_down", "effect_dmg_down", "effect_dmg_down_desc", EffectCategory.DeBuff),
            new("confusion_dmg_down", "effect_confusion_dmg_down", "effect_confusion_dmg_down_desc", EffectCategory.DeBuff),
            new("atk_dmg_down", "effect_atk_dmg_down", "effect_atk_dmg_down_desc", EffectCategory.DeBuff),
            new("res_up", "effect_res_up", "effect_res_up_desc", EffectCategory.DeBuff),
            new("atk_res_up", "effect_atk_res_up", "effect_atk_res_up_desc", EffectCategory.DeBuff),
            new("conf_res_up", "effect_conf_res_up", "effect_conf_res_up_desc", EffectCategory.DeBuff),
            new("heal_rate_down", "effect_heal_rate_down", "effect_heal_rate_down_desc", EffectCategory.DeBuff),
            new("conf_dmg_taken_up", "effect_conf_dmg_taken_up", "effect_conf_dmg_taken_up_desc", EffectCategory.DeBuff),
            new("atk_dmg_taken_up", "effect_atk_dmg_taken_up", "effect_atk_dmg_taken_up_desc", EffectCategory.DeBuff),
            new("dmg_taken_up", "effect_dmg_taken_up", "effect_dmg_taken_up_desc", EffectCategory.DeBuff),
            new("weak", "effect_weak", "effect_weak_desc", EffectCategory.DeBuff),
            new("vulnerable", "effect_vulnerable", "effect_vulnerable_desc", EffectCategory.DeBuff),
            new("clash_power_down", "effect_clash_power_down", "effect_clash_power_down_desc", EffectCategory.DeBuff),
            new("atk_level_down", "effect_atk_level_down", "effect_atk_level_down_desc", EffectCategory.DeBuff),
            new("def_level_down", "effect_def_level_down", "effect_def_level_down_desc", EffectCategory.DeBuff),
            new("dmg_received_up", "effect_dmg_received_up", "effect_dmg_received_up_desc", EffectCategory.DeBuff),
            new("conf_dmg_received_up", "effect_conf_dmg_received_up", "effect_conf_dmg_received_up_desc", EffectCategory.DeBuff),
            new("atk_dmg_received_up", "effect_atk_dmg_received_up", "effect_atk_dmg_received_up_desc", EffectCategory.DeBuff),
            new("burn", "effect_burn", "effect_burn_desc", EffectCategory.DeBuff),
            new("bleed", "effect_bleed", "effect_bleed_desc", EffectCategory.DeBuff),
            new("sink", "effect_sink", "effect_sink_desc", EffectCategory.DeBuff),
            new("paralysis", "effect_paralysis", "effect_paralysis_desc", EffectCategory.DeBuff),
            new("seal", "effect_seal", "effect_seal_desc", EffectCategory.DeBuff),
            new("slow", "effect_slow", "effect_slow_desc", EffectCategory.DeBuff),
            new("fall", "effect_fall", "effect_fall_desc", EffectCategory.DeBuff, maxStack: 10),
            new("fragile", "effect_fragile", "effect_fragile_desc", EffectCategory.DeBuff, maxStack: 10),
            new("rupture", "effect_rupture", "effect_rupture_desc", EffectCategory.DeBuff, hasIntensity: true),
            new("tremor", "effect_tremor", "effect_tremor_desc", EffectCategory.DeBuff),
            new("scorching_tremor", "effect_scorching_tremor", "effect_scorching_tremor_desc", EffectCategory.DeBuff),
            new("flutter", "effect_flutter", "effect_flutter_desc", EffectCategory.DeBuff, hasStack: false),
            new("dice_power_down", "effect_dice_power_down", "effect_dice_power_down_desc", EffectCategory.DeBuff),
            new("poison", "effect_poison", "effect_poison_desc", EffectCategory.DeBuff),
            new("calamity", "effect_calamity", "effect_calamity_desc", EffectCategory.DeBuff),
            new("fatigue", "effect_fatigue", "effect_fatigue_desc", EffectCategory.DeBuff),
            new("frailty", "effect_frailty", "effect_frailty_desc", EffectCategory.DeBuff),
        };

        /// <summary>18 种特殊效果</summary>
        public static readonly StatusEffectData[] SpecialEffects =
        {
            new("power_invuln", "effect_power_invuln", "effect_power_invuln_desc", EffectCategory.Special, hasStack: false),
            new("focus", "effect_focus", "effect_focus_desc", EffectCategory.Special),
            new("conceal", "effect_conceal", "effect_conceal_desc", EffectCategory.Special, hasStack: false),
            new("miracle_power", "effect_miracle_power", "effect_miracle_power_desc", EffectCategory.Special, maxStack: 10),
            new("force_field", "effect_force_field", "effect_force_field_desc", EffectCategory.Special, maxStack: 1000),
            new("qiang_gaze", "effect_qiang_gaze", "effect_qiang_gaze_desc", EffectCategory.Special, maxStack: 10),
            new("qiang_aura", "effect_qiang_aura", "effect_qiang_aura_desc", EffectCategory.Special, maxStack: 10),
            new("stress", "effect_stress", "effect_stress_desc", EffectCategory.Special),
            new("resentment", "effect_resentment", "effect_resentment_desc", EffectCategory.Special),
            new("flower_needle", "effect_flower_needle", "effect_flower_needle_desc", EffectCategory.Special | EffectCategory.Indestructible, maxStack: 3),
            new("mutual_destruction", "effect_mutual_destruction", "effect_mutual_destruction_desc", EffectCategory.Special | EffectCategory.Indestructible, maxStack: 3),
            new("bloodstain", "effect_bloodstain", "effect_bloodstain_desc", EffectCategory.Special | EffectCategory.Indestructible, maxStack: 5),
            new("distortion", "effect_distortion", "effect_distortion_desc", EffectCategory.Special | EffectCategory.Indestructible),
            new("awakening", "effect_awakening", "effect_awakening_desc", EffectCategory.Special | EffectCategory.Indestructible),
            new("bloom", "effect_bloom", "effect_bloom_desc", EffectCategory.Special | EffectCategory.Indestructible),
            new("persist", "effect_persist", "effect_persist_desc", EffectCategory.Special | EffectCategory.Indestructible, hasStack: false),
            new("steadfast", "effect_steadfast", "effect_steadfast_desc", EffectCategory.Special | EffectCategory.Indestructible, hasStack: false),
            new("tough", "effect_tough", "effect_tough_desc", EffectCategory.Special | EffectCategory.Indestructible, hasStack: false),
        };

        /// <summary>5 种群控效果</summary>
        public static readonly StatusEffectData[] CrowdControlEffects =
        {
            new("stun", "effect_stun", "effect_stun_desc", EffectCategory.DeBuff | EffectCategory.CC),
            new("freeze", "effect_freeze", "effect_freeze_desc", EffectCategory.DeBuff | EffectCategory.CC),
            new("confuse", "effect_confuse", "effect_confuse_desc", EffectCategory.DeBuff | EffectCategory.CC, hasStack: false),
            new("taunt", "effect_taunt", "effect_taunt_desc", EffectCategory.CC),
            new("sleep", "effect_sleep", "effect_sleep_desc", EffectCategory.Special | EffectCategory.CC),
        };
    }
}
