using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Core;

public struct EmotionStateRow
{
    public EmotionType Type;
    public EmotionLevel Level;
    public int SanityChange;
    public float DamageRateMod;
    public float DamageTakenRateMod;
    public float HealRateMod;
    public float ConfusionDmgTakenRateMod;
    public float AttackDmgTakenRateMod;
    public float AttackToConfusionConversion;
}

public static class EmotionStateTable
{
    private static readonly List<EmotionStateRow> Rows = new()
    {
        new() { Type = EmotionType.Joy,      Level = EmotionLevel.I,   SanityChange = +1, HealRateMod = 0.10f, ConfusionDmgTakenRateMod = -0.10f },
        new() { Type = EmotionType.Ecstasy,  Level = EmotionLevel.II,  SanityChange = +2, HealRateMod = 0.25f, ConfusionDmgTakenRateMod = -0.25f },
        new() { Type = EmotionType.Mania,    Level = EmotionLevel.III, SanityChange = +3, HealRateMod = 0.50f, ConfusionDmgTakenRateMod = -0.50f },

        new() { Type = EmotionType.Anger,    Level = EmotionLevel.I,   SanityChange = -3, DamageRateMod = 0.20f, DamageTakenRateMod = 0.10f },
        new() { Type = EmotionType.Fury,     Level = EmotionLevel.II,  SanityChange = -5, DamageRateMod = 0.50f, DamageTakenRateMod = 0.25f },
        new() { Type = EmotionType.Rage,     Level = EmotionLevel.III, SanityChange = -10, DamageRateMod = 1.00f, DamageTakenRateMod = 0.50f },

        new() { Type = EmotionType.Sadness,    Level = EmotionLevel.I,   SanityChange = -1, DamageRateMod = -0.10f, AttackDmgTakenRateMod = -0.10f, AttackToConfusionConversion = 0.10f },
        new() { Type = EmotionType.Depression, Level = EmotionLevel.II,  SanityChange = -3, DamageRateMod = -0.25f, AttackDmgTakenRateMod = -0.25f, AttackToConfusionConversion = 0.25f },
        new() { Type = EmotionType.Despair,    Level = EmotionLevel.III, SanityChange = -5, DamageRateMod = -0.50f, AttackDmgTakenRateMod = -0.50f, AttackToConfusionConversion = 0.50f },
    };

    public static EmotionStateRow? Get(EmotionType type) =>
        Rows.FirstOrDefault(r => r.Type == type);
}
