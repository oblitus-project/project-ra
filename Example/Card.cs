using System.Collections.Generic;
using System.Linq;
using ProjectRA.Combat;

namespace ProjectRA.Example
{
	public enum CardRarity
	{
		Normal,
		Special
	}

	public enum SinAttribute
	{
		None,
		Joy,
		Wrath,
		Sadness,
		Fear,
		Pain,
		Collapse
	}

	public struct CardDice
	{
		public int Index;
		public int MinPower;
		public int MaxPower;
		public int LevelBonus;

		public CardDice(int index, int minPower, int maxPower, int levelBonus = 0)
		{
			Index = index;
			MinPower = minPower;
			MaxPower = maxPower;
			LevelBonus = levelBonus;
		}
	}

	public struct CardUpgrade
	{
		public string UpgradeId;
		public int CostIncrease;
		public int DicePowerBonus;

		public CardUpgrade(string upgradeId, int costIncrease = 0, int dicePowerBonus = 0)
		{
			UpgradeId = upgradeId;
			CostIncrease = costIncrease;
			DicePowerBonus = dicePowerBonus;
		}
	}

	public struct CardData
	{
		public string Id;
		public string NameKey;
		public string DescKey;
		public int COST;
		public int SanityCost;
		public int Cooldown;
		public int Copies;
		public CardType Type;
		public CardRarity Rarity;
		public SinAttribute Sin;
		public List<CardDice> Dices;
		public List<string> EffectKeys;
		public List<string> KeywordKeys;
		public CardUpgrade? Upgrade;
		public string ExtraCostLabelKey;

		public string Name => LocalizationManager.Get(NameKey);
		public string Description => LocalizationManager.Get(DescKey);
		public List<string> Effects => EffectKeys?.Select(k => LocalizationManager.Get(k)).ToList();
		public List<string> Keywords => KeywordKeys?.Select(k => LocalizationManager.Get(k)).ToList();
		public string ExtraCostLabel => string.IsNullOrEmpty(ExtraCostLabelKey) ? "" : LocalizationManager.Get(ExtraCostLabelKey);
	}

	public static class CardDB
	{
		private static readonly List<CardDice> GuardDices = new()
		{
			new CardDice(0, 6, 9),
		};

		private static readonly List<CardDice> GuardPlusDices = new()
		{
			new CardDice(0, 9, 13),
		};

		private static readonly List<CardDice> AttackDices = new()
		{
			new CardDice(0, 4, 6, 0),
		};

		private static readonly List<CardDice> SpecialSkillDices = new()
		{
			new CardDice(0, 10, 16, 0),
			new CardDice(1, 8, 12, 0),
		};

		private static readonly List<CardDice> BossSkillDices = new()
		{
			new CardDice(0, 8, 14, 0),
			new CardDice(1, 6, 10, 0),
		};

		public static readonly CardData Guard = new()
		{
			Id = "guard",
			NameKey = "card_guard",
			DescKey = "card_guard_desc",
			COST = 1,
			SanityCost = 0,
			Cooldown = 0,
			Copies = 1,
			Type = CardType.GuardCard,
			Rarity = CardRarity.Normal,
			Sin = SinAttribute.None,
			Dices = GuardDices,
			EffectKeys = new List<string>(),
			KeywordKeys = new List<string> { "card_op_upgrade" },
			Upgrade = new CardUpgrade("guard_plus", 0, 3),
			ExtraCostLabelKey = null,
		};

		public static readonly CardData GuardPlus = new()
		{
			Id = "guard_plus",
			NameKey = "card_guard_plus",
			DescKey = "card_guard_plus_desc",
			COST = 1,
			SanityCost = 0,
			Cooldown = 0,
			Copies = 1,
			Type = CardType.GuardCard,
			Rarity = CardRarity.Normal,
			Sin = SinAttribute.None,
			Dices = GuardPlusDices,
			EffectKeys = new List<string>(),
			KeywordKeys = new List<string>(),
			Upgrade = null,
			ExtraCostLabelKey = null,
		};

		public static readonly CardData Attack = new()
		{
			Id = "attack",
			NameKey = "card_attack",
			DescKey = "card_attack_desc",
			COST = 0,
			SanityCost = 0,
			Cooldown = 0,
			Copies = 1,
			Type = CardType.AttackCard,
			Rarity = CardRarity.Normal,
			Sin = SinAttribute.None,
			Dices = AttackDices,
			EffectKeys = new List<string>(),
			KeywordKeys = new List<string>(),
			Upgrade = null,
			ExtraCostLabelKey = null,
		};

		public static readonly CardData SpecialSkill = new()
		{
			Id = "special_skill",
			NameKey = "card_special_skill",
			DescKey = "card_special_skill_desc",
			COST = 0,
			SanityCost = 0,
			Cooldown = 0,
			Copies = 1,
			Type = CardType.AttackCard,
			Rarity = CardRarity.Special,
			Sin = SinAttribute.Wrath,
			Dices = SpecialSkillDices,
			EffectKeys = new List<string>
			{
				"card_special_skill_effect_0",
				"card_special_skill_effect_1",
			},
			KeywordKeys = new List<string> { "card_op_retain" },
			Upgrade = null,
			ExtraCostLabelKey = null,
		};

		public static readonly CardData ErosionCard = new()
		{
			Id = "erosion_card",
			NameKey = "card_erosion",
			DescKey = "card_erosion_desc",
			COST = 0,
			SanityCost = 0,
			Cooldown = 1,
			Copies = 1,
			Type = CardType.AttackCard,
			Rarity = CardRarity.Special,
			Sin = SinAttribute.Wrath,
			Dices = SpecialSkillDices,
			EffectKeys = new List<string>
			{
				"card_erosion_effect_0",
			},
			KeywordKeys = new List<string>(),
			Upgrade = null,
			ExtraCostLabelKey = "card_cost_label_emotion_erosion",
		};

		public static readonly CardData BossATG = new()
		{
			Id = "boss_atg",
			NameKey = "card_boss_atg",
			DescKey = "card_boss_atg_desc",
			COST = 0,
			SanityCost = 0,
			Cooldown = 1,
			Copies = 1,
			Type = CardType.AttackCard,
			Rarity = CardRarity.Special,
			Sin = SinAttribute.None,
			Dices = BossSkillDices,
			EffectKeys = new List<string>(),
			KeywordKeys = new List<string> { "card_op_retain" },
			Upgrade = null,
			ExtraCostLabelKey = "card_cost_label_atg",
		};

		public static readonly Dictionary<string, CardData> All = new()
		{
			{ "guard", Guard },
			{ "guard_plus", GuardPlus },
			{ "attack", Attack },
			{ "special_skill", SpecialSkill },
			{ "erosion_card", ErosionCard },
			{ "boss_atg", BossATG },
		};
	}
}
