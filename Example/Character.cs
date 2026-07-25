using System.Collections.Generic;
using System.Linq;
using ProjectRA.Combat;

namespace ProjectRA.Example
{
	public enum Difficulty
	{
		Normal,
		Hard,
		Hardcore,
		Extreme,
		Insane,
		Torment,
		Lunatic
	}

	public enum TerrainType
	{
		Urban,
		Wild,
		Indoor
	}

	public enum TerrainRank
	{
		None,
		Low,
		Medium,
		High
	}

	public struct CharacterLevelStats
	{
		public int Level;
		public int HP;
		public int StaggerResist;

		public CharacterLevelStats(int level, int hp, int staggerResist)
		{
			Level = level;
			HP = hp;
			StaggerResist = staggerResist;
		}
	}

	public struct MysteryManifestation
	{
		public MysteryType Type;
		public ArmorType Armor;

		public MysteryManifestation(MysteryType type, ArmorType armor)
		{
			Type = type;
			Armor = armor;
		}
	}

	public struct DamageResistances
	{
		public float Slash;
		public float Pierce;
		public float Blunt;

		public DamageResistances(float slash, float pierce, float blunt)
		{
			Slash = slash;
			Pierce = pierce;
			Blunt = blunt;
		}
	}

	public struct TerrainAdaptabilities
	{
		public TerrainRank Urban;
		public TerrainRank Wild;
		public TerrainRank Indoor;

		public TerrainAdaptabilities(TerrainRank urban, TerrainRank wild, TerrainRank indoor)
		{
			Urban = urban;
			Wild = wild;
			Indoor = indoor;
		}
	}

	public struct GrowthCoefficientData
	{
		public float Coefficient;
		public CharacterLevelStats[] StatsByLevel;

		public CharacterLevelStats GetStats(int level) =>
			StatsByLevel.FirstOrDefault(s => s.Level == level);
	}

	public static class GrowthCoefficientTable
	{
		private static readonly CharacterLevelStats[] Stats_1_11 = new[]
		{
			new CharacterLevelStats(30, 22, 13),
			new CharacterLevelStats(40, 65, 39),
			new CharacterLevelStats(50, 184, 110),
			new CharacterLevelStats(60, 524, 314),
			new CharacterLevelStats(70, 1488, 892),
			new CharacterLevelStats(80, 4225, 2535),
			new CharacterLevelStats(90, 11996, 7198),
		};

		private static readonly CharacterLevelStats[] Stats_1_109 = new[]
		{
			new CharacterLevelStats(30, 22, 13),
			new CharacterLevelStats(40, 62, 36),
			new CharacterLevelStats(50, 176, 104),
			new CharacterLevelStats(60, 496, 292),
			new CharacterLevelStats(70, 1397, 824),
			new CharacterLevelStats(80, 3931, 2319),
			new CharacterLevelStats(90, 11062, 6526),
		};

		private static readonly CharacterLevelStats[] Stats_1_108 = new[]
		{
			new CharacterLevelStats(30, 21, 12),
			new CharacterLevelStats(40, 60, 35),
			new CharacterLevelStats(50, 168, 97),
			new CharacterLevelStats(60, 470, 272),
			new CharacterLevelStats(70, 1311, 760),
			new CharacterLevelStats(80, 3657, 2121),
			new CharacterLevelStats(90, 10199, 5915),
		};

		private static readonly CharacterLevelStats[] Stats_1_107 = new[]
		{
			new CharacterLevelStats(30, 21, 12),
			new CharacterLevelStats(40, 58, 33),
			new CharacterLevelStats(50, 161, 91),
			new CharacterLevelStats(60, 445, 253),
			new CharacterLevelStats(70, 1231, 701),
			new CharacterLevelStats(80, 3402, 1939),
			new CharacterLevelStats(90, 9403, 5359),
		};

		private static readonly CharacterLevelStats[] Stats_1_106 = new[]
		{
			new CharacterLevelStats(30, 20, 11),
			new CharacterLevelStats(40, 56, 31),
			new CharacterLevelStats(50, 154, 86),
			new CharacterLevelStats(60, 421, 236),
			new CharacterLevelStats(70, 1155, 647),
			new CharacterLevelStats(80, 3165, 1772),
			new CharacterLevelStats(90, 8668, 4854),
		};

		private static readonly CharacterLevelStats[] Stats_1_105 = new[]
		{
			new CharacterLevelStats(30, 19, 10),
			new CharacterLevelStats(40, 54, 29),
			new CharacterLevelStats(50, 147, 80),
			new CharacterLevelStats(60, 399, 219),
			new CharacterLevelStats(70, 1084, 596),
			new CharacterLevelStats(80, 2944, 1619),
			new CharacterLevelStats(90, 7991, 4395),
		};

		private static readonly CharacterLevelStats[] Stats_1_104 = new[]
		{
			new CharacterLevelStats(30, 19, 10),
			new CharacterLevelStats(40, 52, 28),
			new CharacterLevelStats(50, 140, 76),
			new CharacterLevelStats(60, 378, 204),
			new CharacterLevelStats(70, 1018, 549),
			new CharacterLevelStats(80, 2738, 1478),
			new CharacterLevelStats(90, 7365, 3977),
		};

		private static readonly CharacterLevelStats[] Stats_1_103 = new[]
		{
			new CharacterLevelStats(30, 18, 10),
			new CharacterLevelStats(40, 50, 26),
			new CharacterLevelStats(50, 134, 71),
			new CharacterLevelStats(60, 358, 190),
			new CharacterLevelStats(70, 955, 506),
			new CharacterLevelStats(80, 2547, 1349),
			new CharacterLevelStats(90, 6788, 3598),
		};

		private static readonly CharacterLevelStats[] Stats_1_102 = new[]
		{
			new CharacterLevelStats(30, 18, 9),
			new CharacterLevelStats(40, 48, 25),
			new CharacterLevelStats(50, 128, 66),
			new CharacterLevelStats(60, 339, 176),
			new CharacterLevelStats(70, 896, 466),
			new CharacterLevelStats(80, 2368, 1231),
			new CharacterLevelStats(90, 6256, 3253),
		};

		private static readonly CharacterLevelStats[] Stats_1_101 = new[]
		{
			new CharacterLevelStats(30, 17, 9),
			new CharacterLevelStats(40, 46, 23),
			new CharacterLevelStats(50, 122, 62),
			new CharacterLevelStats(60, 321, 163),
			new CharacterLevelStats(70, 841, 429),
			new CharacterLevelStats(80, 2202, 1123),
			new CharacterLevelStats(90, 5765, 2940),
		};

		private static readonly CharacterLevelStats[] Stats_1_10 = new[]
		{
			new CharacterLevelStats(30, 17, 8),
			new CharacterLevelStats(40, 45, 22),
			new CharacterLevelStats(50, 117, 58),
			new CharacterLevelStats(60, 304, 152),
			new CharacterLevelStats(70, 789, 394),
			new CharacterLevelStats(80, 2048, 1024),
			new CharacterLevelStats(90, 5313, 2656),
		};

		private static readonly CharacterLevelStats[] Stats_1_099 = new[]
		{
			new CharacterLevelStats(30, 16, 8),
			new CharacterLevelStats(40, 43, 21),
			new CharacterLevelStats(50, 112, 54),
			new CharacterLevelStats(60, 288, 141),
			new CharacterLevelStats(70, 741, 363),
			new CharacterLevelStats(80, 1904, 933),
			new CharacterLevelStats(90, 4895, 2398),
		};

		private static readonly CharacterLevelStats[] Stats_1_098 = new[]
		{
			new CharacterLevelStats(30, 16, 7),
			new CharacterLevelStats(40, 42, 20),
			new CharacterLevelStats(50, 107, 51),
			new CharacterLevelStats(60, 272, 131),
			new CharacterLevelStats(70, 695, 333),
			new CharacterLevelStats(80, 1770, 850),
			new CharacterLevelStats(90, 4510, 2164),
		};

		private static readonly CharacterLevelStats[] Stats_1_097 = new[]
		{
			new CharacterLevelStats(30, 16, 7),
			new CharacterLevelStats(40, 40, 19),
			new CharacterLevelStats(50, 102, 48),
			new CharacterLevelStats(60, 258, 121),
			new CharacterLevelStats(70, 652, 306),
			new CharacterLevelStats(80, 1646, 773),
			new CharacterLevelStats(90, 4155, 1952),
		};

		private static readonly CharacterLevelStats[] Stats_1_096 = new[]
		{
			new CharacterLevelStats(30, 15, 7),
			new CharacterLevelStats(40, 39, 17),
			new CharacterLevelStats(50, 97, 45),
			new CharacterLevelStats(60, 244, 112),
			new CharacterLevelStats(70, 611, 281),
			new CharacterLevelStats(80, 1530, 704),
			new CharacterLevelStats(90, 3827, 1760),
		};

		private static readonly CharacterLevelStats[] Stats_1_095 = new[]
		{
			new CharacterLevelStats(30, 15, 6),
			new CharacterLevelStats(40, 37, 16),
			new CharacterLevelStats(50, 93, 42),
			new CharacterLevelStats(60, 231, 104),
			new CharacterLevelStats(70, 574, 258),
			new CharacterLevelStats(80, 1422, 640),
			new CharacterLevelStats(90, 3525, 1586),
		};

		private static readonly CharacterLevelStats[] Stats_1_094 = new[]
		{
			new CharacterLevelStats(30, 14, 6),
			new CharacterLevelStats(40, 36, 16),
			new CharacterLevelStats(50, 89, 39),
			new CharacterLevelStats(60, 219, 96),
			new CharacterLevelStats(70, 538, 236),
			new CharacterLevelStats(80, 1322, 581),
			new CharacterLevelStats(90, 3247, 1428),
		};

		private static readonly CharacterLevelStats[] Stats_1_093 = new[]
		{
			new CharacterLevelStats(30, 14, 6),
			new CharacterLevelStats(40, 35, 14),
			new CharacterLevelStats(50, 85, 36),
			new CharacterLevelStats(60, 207, 89),
			new CharacterLevelStats(70, 505, 217),
			new CharacterLevelStats(80, 1530, 658),
			new CharacterLevelStats(90, 2990, 1286),
		};

		private static readonly CharacterLevelStats[] Stats_1_092 = new[]
		{
			new CharacterLevelStats(30, 14, 5),
			new CharacterLevelStats(40, 33, 13),
			new CharacterLevelStats(50, 81, 34),
			new CharacterLevelStats(60, 196, 82),
			new CharacterLevelStats(70, 473, 198),
			new CharacterLevelStats(80, 1142, 479),
			new CharacterLevelStats(90, 2754, 1156),
		};

		private static readonly CharacterLevelStats[] Stats_1_091 = new[]
		{
			new CharacterLevelStats(30, 13, 5),
			new CharacterLevelStats(40, 32, 13),
			new CharacterLevelStats(50, 77, 31),
			new CharacterLevelStats(60, 185, 76),
			new CharacterLevelStats(70, 444, 182),
			new CharacterLevelStats(80, 1061, 435),
			new CharacterLevelStats(90, 2536, 1039),
		};

		private static readonly CharacterLevelStats[] Stats_1_09 = new[]
		{
			new CharacterLevelStats(30, 13, 5),
			new CharacterLevelStats(40, 31, 12),
			new CharacterLevelStats(50, 74, 29),
			new CharacterLevelStats(60, 176, 70),
			new CharacterLevelStats(70, 416, 166),
			new CharacterLevelStats(80, 986, 394),
			new CharacterLevelStats(90, 2335, 934),
		};

		private static readonly Dictionary<float, GrowthCoefficientData> _table = new()
		{
			{ 1.11f,  new GrowthCoefficientData { Coefficient = 1.11f,  StatsByLevel = Stats_1_11 } },
			{ 1.109f, new GrowthCoefficientData { Coefficient = 1.109f, StatsByLevel = Stats_1_109 } },
			{ 1.108f, new GrowthCoefficientData { Coefficient = 1.108f, StatsByLevel = Stats_1_108 } },
			{ 1.107f, new GrowthCoefficientData { Coefficient = 1.107f, StatsByLevel = Stats_1_107 } },
			{ 1.106f, new GrowthCoefficientData { Coefficient = 1.106f, StatsByLevel = Stats_1_106 } },
			{ 1.105f, new GrowthCoefficientData { Coefficient = 1.105f, StatsByLevel = Stats_1_105 } },
			{ 1.104f, new GrowthCoefficientData { Coefficient = 1.104f, StatsByLevel = Stats_1_104 } },
			{ 1.103f, new GrowthCoefficientData { Coefficient = 1.103f, StatsByLevel = Stats_1_103 } },
			{ 1.102f, new GrowthCoefficientData { Coefficient = 1.102f, StatsByLevel = Stats_1_102 } },
			{ 1.101f, new GrowthCoefficientData { Coefficient = 1.101f, StatsByLevel = Stats_1_101 } },
			{ 1.10f,  new GrowthCoefficientData { Coefficient = 1.10f,  StatsByLevel = Stats_1_10 } },
			{ 1.099f, new GrowthCoefficientData { Coefficient = 1.099f, StatsByLevel = Stats_1_099 } },
			{ 1.098f, new GrowthCoefficientData { Coefficient = 1.098f, StatsByLevel = Stats_1_098 } },
			{ 1.097f, new GrowthCoefficientData { Coefficient = 1.097f, StatsByLevel = Stats_1_097 } },
			{ 1.096f, new GrowthCoefficientData { Coefficient = 1.096f, StatsByLevel = Stats_1_096 } },
			{ 1.095f, new GrowthCoefficientData { Coefficient = 1.095f, StatsByLevel = Stats_1_095 } },
			{ 1.094f, new GrowthCoefficientData { Coefficient = 1.094f, StatsByLevel = Stats_1_094 } },
			{ 1.093f, new GrowthCoefficientData { Coefficient = 1.093f, StatsByLevel = Stats_1_093 } },
			{ 1.092f, new GrowthCoefficientData { Coefficient = 1.092f, StatsByLevel = Stats_1_092 } },
			{ 1.091f, new GrowthCoefficientData { Coefficient = 1.091f, StatsByLevel = Stats_1_091 } },
			{ 1.09f,  new GrowthCoefficientData { Coefficient = 1.09f,  StatsByLevel = Stats_1_09 } },
		};

		public static GrowthCoefficientData Get(float coefficient) =>
			_table.TryGetValue(coefficient, out var data) ? data : default;

		public static IEnumerable<float> AllCoefficients => _table.Keys.OrderByDescending(k => k);
	}

	public struct CharacterBaseStats
	{
		public int SPD;
		public MysteryManifestation Manifestation;
		public DamageResistances Resistances;
		public TerrainAdaptabilities Terrain;

		public CharacterBaseStats(int spd, MysteryManifestation manifestation,
			DamageResistances resistances, TerrainAdaptabilities terrain)
		{
			SPD = spd;
			Manifestation = manifestation;
			Resistances = resistances;
			Terrain = terrain;
		}
	}

	public struct CharacterTemplate
	{
		public string Id;
		public string NameKey;
		public string DescKey;
		public string Faction;
		public string Region;
		public GrowthCoefficientData Growth;
		public CharacterBaseStats BaseStats;
		public bool IsBoss;
		public Dictionary<Difficulty, int> BossLevelMap;

		public string Name => LocalizationManager.Get(NameKey);
		public string Description => LocalizationManager.Get(DescKey);

		public CharacterLevelStats GetLevelStats(int level) =>
			Growth.GetStats(level);
	}
}
