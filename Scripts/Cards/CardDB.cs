using System.Collections.Generic;
using ProjectRA.Combat;

namespace ProjectRA.Cards;

public static class CardDB
{
    private static readonly Dictionary<string, CardData> _cards = new();
    private static bool _initialized;

    public static IReadOnlyDictionary<string, CardData> All => _cards;

    public static void Init()
    {
        if (_initialized) return;
        RegisterAll();
        _initialized = true;
    }

    public static CardData Get(string id)
    {
        Init();
        return _cards.TryGetValue(id, out var card) ? card : default;
    }

    public static CardData[] GetMany(params string[] ids)
    {
        Init();
        var result = new CardData[ids.Length];
        for (int i = 0; i < ids.Length; i++)
            result[i] = Get(ids[i]);
        return result;
    }

    private static void RegisterAll()
    {
        Register(Guard);
        Register(GuardPlus);
        Register(Slash);
        Register(Pierce);
        Register(Blunt);
        Register(UnstoppableOffense);
        Register(Breakthrough);
        Register(RageOutburst);
        Register(Burn);
        Register(EmberSource);
        Register(Brutality);
        Register(Frenzy);
        Register(Resist);
        Register(Beatdown);
        Register(HeavyHammer);
        Register(PommelStrike);
        Register(ToughItOut);
        Register(PowerStrike);
    }

    private static void Register(CardData card)
    {
        _cards[card.Id] = card;
    }

    private static DiceInstance MakeDice(DiceType type, int min, int max,
        AttackAttribute attr = AttackAttribute.Slash,
        AttackRange range = AttackRange.Melee,
        int levelMod = 0,
        bool indestructible = false,
        int aw = 0, int hw = 0)
    {
        return new DiceInstance
        {
            Type = type,
            SubType = DiceSubType.Normal,
            Attribute = attr,
            Range = range,
            BaseMin = min,
            BaseMax = max,
            LevelMod = levelMod,
            IsIndestructible = indestructible,
            AwValue = aw,
            HwValue = hw,
        };
    }

    private static readonly DiceInstance[] GuardDices =
    {
        MakeDice(DiceType.Defensive, 6, 9, levelMod: 0),
    };

    private static readonly DiceInstance[] GuardPlusDices =
    {
        MakeDice(DiceType.Defensive, 9, 13, levelMod: 0),
    };

    private static readonly DiceInstance[] SlashDices =
    {
        MakeDice(DiceType.Offensive, 5, 7, AttackAttribute.Slash, AttackRange.Melee),
    };

    private static readonly DiceInstance[] PierceDices =
    {
        MakeDice(DiceType.Offensive, 5, 7, AttackAttribute.Pierce, AttackRange.Melee),
    };

    private static readonly DiceInstance[] BluntDices =
    {
        MakeDice(DiceType.Offensive, 5, 7, AttackAttribute.Blunt, AttackRange.Melee),
    };

    private static readonly DiceInstance[] UnstoppableDices =
    {
        MakeDice(DiceType.Offensive, 7, 9, AttackAttribute.Blunt, AttackRange.Melee, 1),
        MakeDice(DiceType.Offensive, 6, 9, AttackAttribute.Blunt, AttackRange.Melee, 1),
    };

    private static readonly DiceInstance[] BreakthroughDices =
    {
        MakeDice(DiceType.MassAttack, 6, 10, AttackAttribute.Slash, AttackRange.MassIndividual, 3),
    };

    private static readonly DiceInstance[] BeatdownDices =
    {
        MakeDice(DiceType.Offensive, 5, 7, AttackAttribute.Blunt, AttackRange.Melee, 5),
        MakeDice(DiceType.Offensive, 5, 7, AttackAttribute.Blunt, AttackRange.Melee, 5),
    };

    private static readonly DiceInstance[] HeavyHammerDices =
    {
        MakeDice(DiceType.Offensive, 6, 18, AttackAttribute.Blunt, AttackRange.Melee, 5),
    };

    private static readonly DiceInstance[] PommelDices =
    {
        MakeDice(DiceType.Offensive, 7, 9, AttackAttribute.Slash, AttackRange.Melee, 1),
    };

    private static readonly DiceInstance[] ToughDices =
    {
        MakeDice(DiceType.Defensive, 16, 20, levelMod: 2),
    };

    private static readonly DiceInstance[] PowerStrikeDices =
    {
        MakeDice(DiceType.Offensive, 9, 15, AttackAttribute.Blunt, AttackRange.Melee, 5),
    };

    private static readonly DiceInstance[] ResistDices =
    {
        MakeDice(DiceType.Defensive, 6, 9, levelMod: 2),
    };

    public static readonly CardData Guard = new()
    {
        Id = "guard",
        NameKey = "card_guard",
        DescKey = "card_guard_desc",
        Cost = 1,
        Type = CardType.GuardCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Dices = GuardDices,
        Keywords = CardKeyword.Upgrade,
        Upgrade = new CardUpgrade("guard_plus", 0, 3),
    };

    public static readonly CardData GuardPlus = new()
    {
        Id = "guard_plus",
        NameKey = "card_guard_plus",
        DescKey = "card_guard_plus_desc",
        Cost = 1,
        Type = CardType.GuardCard,
        Rarity = CardRarity.Normal,
        Copies = 0,
        Dices = GuardPlusDices,
    };

    public static readonly CardData Slash = new()
    {
        Id = "slash",
        NameKey = "card_slash",
        DescKey = "card_slash_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Dices = SlashDices,
    };

    public static readonly CardData Pierce = new()
    {
        Id = "pierce",
        NameKey = "card_pierce",
        DescKey = "card_pierce_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Dices = PierceDices,
    };

    public static readonly CardData Blunt = new()
    {
        Id = "blunt",
        NameKey = "card_blunt",
        DescKey = "card_blunt_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Dices = BluntDices,
    };

    public static readonly CardData UnstoppableOffense = new()
    {
        Id = "unstoppable_offense",
        NameKey = "card_unstoppable_offense",
        DescKey = "card_unstoppable_offense_desc",
        Cost = 2,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Sin = SinAttribute.Wrath,
        Dices = UnstoppableDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.OnEquip, "card_restore_cost_2"),
            new CardEffectEntry(EffectTiming.OnHit, "card_apply_slow_1"),
            new CardEffectEntry(EffectTiming.OnHit, "card_gain_free_attack_1"),
        },
    };

    public static readonly CardData Breakthrough = new()
    {
        Id = "breakthrough",
        NameKey = "card_breakthrough",
        DescKey = "card_breakthrough_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Uncommon,
        Copies = 2,
        Dices = BreakthroughDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.OnUse, "card_lose_1_hp"),
            new CardEffectEntry(EffectTiming.OnHit, "card_apply_bleed_2"),
        },
    };

    public static readonly CardData RageOutburst = new()
    {
        Id = "rage_outburst",
        NameKey = "card_rage_outburst",
        DescKey = "card_rage_outburst_desc",
        Cost = 1,
        Type = CardType.AbilityCard,
        Rarity = CardRarity.Uncommon,
        Copies = 1,
        Sin = SinAttribute.Wrath,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.TurnStart, "card_lose_1_hp"),
            new CardEffectEntry(EffectTiming.OnDamageTaken, "card_deal_6_dmg_all_enemies"),
        },
    };

    public static readonly CardData Burn = new()
    {
        Id = "burn",
        NameKey = "card_burn",
        DescKey = "card_burn_desc",
        Cost = 1,
        Type = CardType.AbilityCard,
        Rarity = CardRarity.Uncommon,
        Copies = 1,
        Sin = SinAttribute.Wrath,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.BattleStart, "card_gain_2_power"),
        },
    };

    public static readonly CardData EmberSource = new()
    {
        Id = "ember_source",
        NameKey = "card_ember_source",
        DescKey = "card_ember_source_desc",
        Cost = 2,
        Type = CardType.AbilityCard,
        Rarity = CardRarity.Uncommon,
        Copies = 1,
        Sin = SinAttribute.Gluttony,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.TurnStart, "card_restore_1_cost"),
        },
    };

    public static readonly CardData Brutality = new()
    {
        Id = "brutality",
        NameKey = "card_brutality",
        DescKey = "card_brutality_desc",
        Cost = 1,
        Type = CardType.AbilityCard,
        Rarity = CardRarity.Uncommon,
        Copies = 1,
        Sin = SinAttribute.Pride,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.TurnStart, "card_lose_1_hp_draw_1"),
        },
    };

    public static readonly CardData Frenzy = new()
    {
        Id = "frenzy",
        NameKey = "card_frenzy",
        DescKey = "card_frenzy_desc",
        Cost = 0,
        Type = CardType.SkillCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.OnUse, "card_gain_shield_per_attack"),
        },
    };

    public static readonly CardData Resist = new()
    {
        Id = "resist",
        NameKey = "card_resist",
        DescKey = "card_resist_desc",
        Cost = 1,
        Type = CardType.GuardCard,
        Rarity = CardRarity.Uncommon,
        Copies = 2,
        Dices = ResistDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.WhileEquipped, "card_resist_50_vs_vulnerable"),
        },
    };

    public static readonly CardData Beatdown = new()
    {
        Id = "beatdown",
        NameKey = "card_beatdown",
        DescKey = "card_beatdown_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Uncommon,
        Copies = 1,
        Sin = SinAttribute.Wrath,
        Dices = BeatdownDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.AfterAttack, "card_exhaust_attack_add_dice"),
        },
    };

    public static readonly CardData HeavyHammer = new()
    {
        Id = "heavy_hammer",
        NameKey = "card_heavy_hammer",
        DescKey = "card_heavy_hammer_desc",
        Cost = 3,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Uncommon,
        Copies = 1,
        Sin = SinAttribute.Wrath,
        Dices = HeavyHammerDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.BeforeAttack, "card_dmg_up_20_if_target_hp_above_50"),
        },
    };

    public static readonly CardData PommelStrike = new()
    {
        Id = "pommel_strike",
        NameKey = "card_pommel_strike",
        DescKey = "card_pommel_strike_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Dices = PommelDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.OnUse, "card_draw_2"),
        },
    };

    public static readonly CardData ToughItOut = new()
    {
        Id = "tough_it_out",
        NameKey = "card_tough_it_out",
        DescKey = "card_tough_it_out_desc",
        Cost = 1,
        Type = CardType.GuardCard,
        Rarity = CardRarity.Normal,
        Copies = 3,
        Dices = ToughDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.OnUse, "card_add_2_wounds_to_hand"),
        },
    };

    public static readonly CardData PowerStrike = new()
    {
        Id = "power_strike",
        NameKey = "card_power_strike",
        DescKey = "card_power_strike_desc",
        Cost = 1,
        Type = CardType.AttackCard,
        Rarity = CardRarity.Normal,
        Copies = 1,
        Sin = SinAttribute.Wrath,
        Dices = PowerStrikeDices,
        Effects = new[]
        {
            new CardEffectEntry(EffectTiming.OnHit, "card_apply_fragile_7"),
        },
        Keywords = CardKeyword.Exhaust,
    };
}
