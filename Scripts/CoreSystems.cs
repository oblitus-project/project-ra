using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Core
{
	/// <summary>
	/// 情绪类型。每种情绪有三级强度，按"开心/狂喜/癫狂"、"生气/愤怒/狂怒"、"悲伤/抑郁/绝望"分组。
	/// 对应 doc/guide/library.md §情绪
	/// </summary>
	public enum EmotionType
	{
		None,
		Joy,       // 开心 (I)
		Ecstasy,   // 狂喜 (II)
		Mania,     // 癫狂 (III)
		Anger,     // 生气 (I)
		Fury,      // 愤怒 (II)
		Rage,      // 狂怒 (III)
		Sadness,   // 悲伤 (I)
		Depression,// 抑郁 (II)
		Despair    // 绝望 (III)
	}

	/// <summary>情绪强度等级</summary>
	public enum EmotionLevel
	{
		I = 1,
		II = 2,
		III = 3
	}

	/// <summary>情绪大类：觉醒情绪 vs 崩溃情绪</summary>
	public enum EmotionCategory
	{
		Awakening,
		Collapse
	}

	/// <summary>情感等级数据行。对应 library.md §情感等级 表格</summary>
	public struct EmotionLevelRow
	{
		public int Level;           // 情感等级 0~5
		public int MaxEmotion;      // 可累积情感上限（-1 表示 Lv.V 无上限）
		public int CostCap;         // COST 上限
		public int SpeedDiceCount;  // 速度骰子数量
		public int SpeedBonus;      // 速度加成
		public int AtkDefBonus;     // 攻防等级加成
		public int CostRecovery;    // 每回合恢复 COST
		public int DrawCount;       // 每回合抽牌数

		public EmotionLevelRow(int level, int maxEmotion, int costCap, int speedDiceCount,
			int speedBonus, int atkDefBonus, int costRecovery, int drawCount)
		{
			Level = level;
			MaxEmotion = maxEmotion;
			CostCap = costCap;
			SpeedDiceCount = speedDiceCount;
			SpeedBonus = speedBonus;
			AtkDefBonus = atkDefBonus;
			CostRecovery = costRecovery;
			DrawCount = drawCount;
		}
	}

	/// <summary>情感等级配置表（Lv.0 ~ Lv.V）</summary>
	public static class EmotionLevelTable
	{
		private static readonly List<EmotionLevelRow> Rows = new()
		{
			new (0, 3, 3, 1, 0, 0, 1, 1),
			new (1, 9, 4, 1, 0, 1, 1, 1),
			new (2, 15, 5, 1, 1, 2, 1, 1),
			new (3, 21, 6, 2, 1, 4, 1, 2),
			new (4, 30, 7, 2, 2, 6, 1, 2),
			new (5, -1, 8, 3, 3, 10, 2, 3),
		};

		public static EmotionLevelRow Get(int level) =>
			Rows.FirstOrDefault(r => r.Level == level);

		/// <summary>判断是否可以从当前等级升级（情感值是否达到上限）</summary>
		public static bool CanLevelUp(int currentLevel, int accumulatedEmotion)
		{
			var row = Get(currentLevel);
			return row.MaxEmotion >= 0 && accumulatedEmotion >= row.MaxEmotion;
		}
	}

	/// <summary>情绪状态数据行。对应 library.md 中开心/生气/悲伤三张表格</summary>
	public struct EmotionStateRow
	{
		public EmotionType Type;
		public EmotionLevel Level;
		public int SanityChange;                     // 理智变化
		public float DamageRateMod;                  // 伤害率修正
		public float DamageTakenRateMod;             // 承伤率修正
		public float HealRateMod;                    // 受回复率修正（仅开心系）
		public float ConfusionDmgTakenRateMod;       // 混乱承伤率修正
		public float AttackDmgTakenRateMod;          // 攻击承伤率修正（仅悲伤系）
		public float AttackToConfusionConversion;    // 攻击伤害→混乱伤害转化率（仅悲伤系）
	}

	/// <summary>情绪状态配置表，含 9 种情绪的三级数据</summary>
	public static class EmotionStateTable
	{
		private static readonly List<EmotionStateRow> Rows = new()
		{
			// 开心系（觉醒情绪）：恢复理智 + 受回复率↑ 混乱承伤率↓
			new() { Type = EmotionType.Joy,      Level = EmotionLevel.I,   SanityChange = +1, HealRateMod = 0.10f, ConfusionDmgTakenRateMod = -0.10f },
			new() { Type = EmotionType.Ecstasy,  Level = EmotionLevel.II,  SanityChange = +2, HealRateMod = 0.25f, ConfusionDmgTakenRateMod = -0.25f },
			new() { Type = EmotionType.Mania,    Level = EmotionLevel.III, SanityChange = +3, HealRateMod = 0.50f, ConfusionDmgTakenRateMod = -0.50f },

			// 生气系（觉醒情绪）：失去理智 + 伤害率↑ 承伤率↑
			new() { Type = EmotionType.Anger,    Level = EmotionLevel.I,   SanityChange = -3, DamageRateMod = 0.20f, DamageTakenRateMod = 0.10f },
			new() { Type = EmotionType.Fury,     Level = EmotionLevel.II,  SanityChange = -5, DamageRateMod = 0.50f, DamageTakenRateMod = 0.25f },
			new() { Type = EmotionType.Rage,     Level = EmotionLevel.III, SanityChange = -10, DamageRateMod = 1.00f, DamageTakenRateMod = 0.50f },

			// 悲伤系（觉醒情绪）：失去理智 + 伤害率↓ 部分攻击伤→混乱伤
			new() { Type = EmotionType.Sadness,    Level = EmotionLevel.I,   SanityChange = -1, DamageRateMod = -0.10f, AttackDmgTakenRateMod = -0.10f, AttackToConfusionConversion = 0.10f },
			new() { Type = EmotionType.Depression, Level = EmotionLevel.II,  SanityChange = -3, DamageRateMod = -0.25f, AttackDmgTakenRateMod = -0.25f, AttackToConfusionConversion = 0.25f },
			new() { Type = EmotionType.Despair,    Level = EmotionLevel.III, SanityChange = -5, DamageRateMod = -0.50f, AttackDmgTakenRateMod = -0.50f, AttackToConfusionConversion = 0.50f },
		};

		public static EmotionStateRow? Get(EmotionType type) =>
			Rows.FirstOrDefault(r => r.Type == type);
	}

	/// <summary>情绪侵蚀条件数据。对应 library.md §情绪侵蚀</summary>
	public struct EmotionErosionData
	{
		public EmotionType RequiredEmotion;   // 前置情绪（狂喜+ / 愤怒+ / 抑郁+）
		public int RequiredSanity;            // 理智要求（+50 或 -50）
		public EmotionType ErosionType;       // 侵蚀后的情绪类型
		public bool RequiresEfwSkill;         // 是否需要 EFW 技能才能触发
		public bool RandomAttack;             // 是否无差别攻击（癫狂侵蚀除外）
	}

	/// <summary>情绪侵蚀配置表</summary>
	public static class EmotionErosionTable
	{
		public static readonly List<EmotionErosionData> Entries = new()
		{
			new() { RequiredEmotion = EmotionType.Ecstasy, RequiredSanity = +50, ErosionType = EmotionType.Mania,  RequiresEfwSkill = true, RandomAttack = false },
			new() { RequiredEmotion = EmotionType.Fury,    RequiredSanity = -50, ErosionType = EmotionType.Rage,   RequiresEfwSkill = true, RandomAttack = true },
			new() { RequiredEmotion = EmotionType.Depression, RequiredSanity = -50, ErosionType = EmotionType.Despair, RequiresEfwSkill = true, RandomAttack = true },
		};

		/// <summary>查找匹配的侵蚀配置。若当前情绪和理智满足条件，返回对应侵蚀数据</summary>
		public static EmotionErosionData? GetErosion(EmotionType emotion, int sanity)
		{
			foreach (var e in Entries)
				if (e.RequiredEmotion == emotion && sanity == e.RequiredSanity)
					return e;
			return null;
		}
	}

	/// <summary>情绪关系表。情绪等级差距决定迅捷一击/疲软一击的比率</summary>
	public static class EmotionRelationTable
	{
		/// <summary>根据情绪等级差距获取迅捷一击（伤害率增加）</summary>
		public static float GetAdvantageRate(int levelGap)
		{
			return levelGap switch
			{
				2 => 0.10f,
				3 => 0.25f,
				4 => 0.50f,
				5 => 0.75f,
				>= 6 => 1.00f,
				_ => 0f
			};
		}

		/// <summary>疲软一击（伤害率降低），数值与迅捷一击相同</summary>
		public static float GetDisadvantageRate(int levelGap) => GetAdvantageRate(levelGap);
	}

	/// <summary>
	/// 理智系统。区间 [-50, +50]。
	/// - 高理智：移除骰子范围的低端值，并倾向掷出高值
	/// - 低理智：移除骰子范围的高端值，并倾向掷出低值
	/// 详见 library.md §理智
	/// </summary>
	public static class SanitySystem
	{
		public const int MinSanity = -50;
		public const int MaxSanity = 50;
		/// <summary>裁剪比例常数 R = 4，表示最多移除骰子范围的 1/4</summary>
		public const float BalanceConstantR = 4f;

		/// <summary>理智影响下的骰子概率分布结果</summary>
		public struct DiceWeightResult
		{
			public int EffectiveMin;   // 有效区间最小值
			public int EffectiveMax;   // 有效区间最大值
			public float[] Weights;    // 各值的权重
		}

		/// <summary>计算给定骰子范围 + 理智下的概率权重分布</summary>
		public static DiceWeightResult CalculateDiceWeights(int min, int max, int sanity)
		{
			int n = max - min + 1;
			// k = floor(|sanity|/50 * n/R)
			int k = (int)Math.Floor(Math.Abs(sanity) / 50.0 * n / BalanceConstantR);

			// 确定有效区间：高理智移除低值，低理智移除高值
			int effMin = sanity >= 0 ? min + k : min;
			int effMax = sanity >= 0 ? max : max - k;

			if (effMin > effMax)
			{
				int mid = (min + max) / 2;
				effMin = effMax = mid;
			}

			// 计算权重：w(v) = 1 + (sanity/50) * ((v-a)/(b-a) - 0.5)
			float[] weights = new float[effMax - effMin + 1];
			for (int i = 0; i < weights.Length; i++)
			{
				int v = effMin + i;
				float t = (float)(v - min) / (max - min);
				float x = sanity / 50f;
				weights[i] = 1f + x * (t - 0.5f);
				if (weights[i] < 0) weights[i] = 0;
			}

			return new DiceWeightResult
			{
				EffectiveMin = effMin,
				EffectiveMax = effMax,
				Weights = weights
			};
		}

		// ---- 理智变化常量 ----
		public static int SanityOnClashWin => 5;
		public static int SanityOnClashLose => -2;

		/// <summary>陷入混乱时失去的理智（随情绪等级增加）</summary>
		public static int SanityOnConfusion(int emotionLevel) =>
			emotionLevel switch { 1 => -3, 2 => -5, 3 => -10, _ => -3 };

		/// <summary>击杀敌方时恢复的理智：15 + (等级差)/3</summary>
		public static int SanityOnKill(int level, int targetLevel) =>
			15 + (level - targetLevel) / 3;

		/// <summary>友方倒下时失去的理智：-(15 + (等级差)/3)</summary>
		public static int SanityOnAllyDown(int level, int targetLevel) =>
			-(15 + (level - targetLevel) / 3);
	}

	/// <summary>速度骰子。每回合投掷，得到速度值，作为卡牌载体</summary>
	public struct SpeedDice
	{
		public int Speed;             // 速度值（决定行动顺序）
		public bool IsDestroyed;      // 是否被破坏（本回合不能使用）
		public bool IsCracked;        // 是否破碎（不可破坏的骰子拼点失败后的状态）
		public int CrackedFixedValue; // 破碎状态下的固定威力值
	}

	/// <summary>速度骰子池。管理角色的所有速度骰子</summary>
	public class SpeedDicePool
	{
		public List<SpeedDice> Dice { get; private set; } = new();
		public int Count => Dice.Count;
		public int AvailableCount => Dice.Count(d => !d.IsDestroyed);

		/// <summary>投掷速度骰子：生成指定数量的骰子，按速度从大到小排序</summary>
		public void Roll(int diceCount, int minSpeed, int maxSpeed, Random rng)
		{
			Dice.Clear();
			for (int i = 0; i < diceCount; i++)
				Dice.Add(new SpeedDice
				{
					Speed = rng.Next(minSpeed, maxSpeed + 1),
					IsDestroyed = false,
					IsCracked = false
				});
			Dice = Dice.OrderByDescending(d => d.Speed).ToList();
		}

		/// <summary>摧毁指定索引的骰子</summary>
		public void Destroy(int index)
		{
			if (index >= 0 && index < Dice.Count)
				Dice[index] = Dice[index] with { IsDestroyed = true };
		}

		/// <summary>将指定索引的骰子变为破碎状态</summary>
		public void Crack(int index)
		{
			if (index >= 0 && index < Dice.Count && !Dice[index].IsDestroyed)
				Dice[index] = Dice[index] with { IsCracked = true };
		}

		/// <summary>重置所有骰子的状态（新回合调用）</summary>
		public void Reset()
		{
			for (int i = 0; i < Dice.Count; i++)
				Dice[i] = Dice[i] with { IsDestroyed = false, IsCracked = false };
		}
	}

	/// <summary>
	/// COST 系统。打出卡牌需要的资源。
	/// 每回合恢复点数由情感等级决定；升级时回满 COST。
	/// 使目标混乱或击杀也可恢复 1 COST。
	/// </summary>
	public class CostSystem
	{
		public int CurrentCost { get; private set; }
		public int MaxCost { get; private set; }
		public int EmotionLevel { get; private set; }

		/// <summary>初始为 Lv.0，COST 上限 3，满额</summary>
		public CostSystem()
		{
			var row = EmotionLevelTable.Get(0);
			MaxCost = row.CostCap;
			CurrentCost = MaxCost;
		}

		/// <summary>回合开始时恢复 COST（恢复量取决于当前情感等级）</summary>
		public void OnTurnStart()
		{
			var row = EmotionLevelTable.Get(EmotionLevel);
			CurrentCost = Math.Min(CurrentCost + row.CostRecovery, MaxCost);
		}

		/// <summary>消耗 COST。不足时返回 false</summary>
		public bool Spend(int amount)
		{
			if (CurrentCost < amount) return false;
			CurrentCost -= amount;
			return true;
		}

		/// <summary>情感等级提升：COST 上限 +1 并回满</summary>
		public void OnEmotionLevelUp()
		{
			EmotionLevel++;
			var row = EmotionLevelTable.Get(EmotionLevel);
			MaxCost = row.CostCap;
			CurrentCost = MaxCost;
		}

		/// <summary>击杀或使目标混乱时恢复 1 COST</summary>
		public void OnKillOrConfusion()
		{
			CurrentCost = Math.Min(CurrentCost + 1, MaxCost);
		}
	}

	/// <summary>角色当前的情绪状态</summary>
	public struct EmotionState
	{
		public EmotionType Type;           // 情绪类型
		public EmotionCategory Category;   // 情绪大类
		public int EmotionLevelCount;      // 情绪等级计数（决定升级所需次数）
	}
}
