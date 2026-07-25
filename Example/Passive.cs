using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Example
{
	public enum PassiveTrigger
	{
		Permanent,
		TurnStart,
		TurnEnd,
		TurnEndEveryN,
		ClashWin,
		ClashLose,
		OnKill,
		OnDowned,
		OnStaggered,
		OnEvade,
		RoundStart,
		RoundEnd,
		OnDeploy,
		OnRetreat
	}

	public enum ConditionTarget
	{
		None,
		DifficultyMin,
		EmotionLevelMin,
		EmotionLevelExact,
		HasStatusEffect,
		HpBelowPercent,
		IsBoss
	}

	public struct PassiveCondition
	{
		public ConditionTarget Type;
		public string Value;

		public PassiveCondition(ConditionTarget type, string value)
		{
			Type = type;
			Value = value;
		}
	}

	public struct PassiveStackLimit
	{
		public int MaxStacks;
		public bool IsVisible;

		public PassiveStackLimit(int maxStacks, bool isVisible = true)
		{
			MaxStacks = maxStacks;
			IsVisible = isVisible;
		}
	}

	public struct PassiveData
	{
		public string Id;
		public string NameKey;
		public string DescKey;
		public string FlavorKey;
		public PassiveTrigger Trigger;
		public int TriggerInterval;
		public List<PassiveCondition> Conditions;
		public PassiveStackLimit StackLimit;
		public List<string> EffectKeys;

		public string Name => LocalizationManager.Get(NameKey);
		public string Description => LocalizationManager.Get(DescKey);
		public string Flavor => string.IsNullOrEmpty(FlavorKey) ? "" : LocalizationManager.Get(FlavorKey);
		public List<string> Effects => EffectKeys.Select(k => LocalizationManager.Get(k)).ToList();
	}

	public static class PassiveDB
	{
		private static readonly List<string> FastBattleEffectKeys = new()
		{
			"passive_fast_battle_effect_0",
			"passive_fast_battle_effect_1",
		};

		private static readonly List<string> ATGChargeEffectKeys = new()
		{
			"passive_atg_charge_effect_0",
		};

		private static readonly List<string> BattleSupportEffectKeys = new()
		{
			"passive_battle_support_effect_0",
		};

		public static readonly PassiveData FastBattle3 = new()
		{
			Id = "fast_battle_3",
			NameKey = "passive_fast_battle_3",
			DescKey = "passive_fast_battle_3_desc",
			FlavorKey = null,
			Trigger = PassiveTrigger.Permanent,
			TriggerInterval = 0,
			Conditions = new List<PassiveCondition>(),
			StackLimit = new PassiveStackLimit(0, false),
			EffectKeys = FastBattleEffectKeys,
		};

		public static readonly PassiveData ATGCharge = new()
		{
			Id = "atg_charge",
			NameKey = "passive_atg_charge",
			DescKey = "passive_atg_charge_desc",
			FlavorKey = null,
			Trigger = PassiveTrigger.TurnEndEveryN,
			TriggerInterval = 3,
			Conditions = new List<PassiveCondition>(),
			StackLimit = new PassiveStackLimit(3, true),
			EffectKeys = ATGChargeEffectKeys,
		};

		public static readonly PassiveData BattleSupport = new()
		{
			Id = "battle_support",
			NameKey = "passive_battle_support",
			DescKey = "passive_battle_support_desc",
			FlavorKey = null,
			Trigger = PassiveTrigger.TurnStart,
			TriggerInterval = 0,
			Conditions = new List<PassiveCondition>
			{
				new PassiveCondition(ConditionTarget.DifficultyMin, "Insane"),
			},
			StackLimit = new PassiveStackLimit(0, false),
			EffectKeys = BattleSupportEffectKeys,
		};

		public static readonly Dictionary<string, PassiveData> All = new()
		{
			{ "fast_battle_3", FastBattle3 },
			{ "atg_charge", ATGCharge },
			{ "battle_support", BattleSupport },
		};
	}
}
