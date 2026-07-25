using System;
using System.Collections.Generic;

namespace ProjectRA.Combat;

public static class DamageCalculator
{
    private static readonly Dictionary<(ArmorType, MysteryType), float> MysteryTable = new()
    {
        { (ArmorType.LightArmor,     MysteryType.Explosive),  1.5f },
        { (ArmorType.HeavyArmor,     MysteryType.Explosive),  1.0f },
        { (ArmorType.CompositeArmor, MysteryType.Explosive),  1.0f },
        { (ArmorType.SpecialArmor,   MysteryType.Explosive),  0.5f },
        { (ArmorType.ElasticArmor,   MysteryType.Explosive),  0.5f },
        { (ArmorType.NormalArmor,    MysteryType.Explosive),  1.0f },
        { (ArmorType.Structure,      MysteryType.Explosive),  0.5f },

        { (ArmorType.LightArmor,     MysteryType.Piercing),   0.5f },
        { (ArmorType.HeavyArmor,     MysteryType.Piercing),   1.5f },
        { (ArmorType.CompositeArmor, MysteryType.Piercing),   1.0f },
        { (ArmorType.SpecialArmor,   MysteryType.Piercing),   1.0f },
        { (ArmorType.ElasticArmor,   MysteryType.Piercing),   1.0f },
        { (ArmorType.NormalArmor,    MysteryType.Piercing),   1.0f },
        { (ArmorType.Structure,      MysteryType.Piercing),   0.5f },

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

    public static float GetMysteryMultiplier(ArmorType armor, MysteryType mysteryType)
    {
        if (MysteryTable.TryGetValue((armor, mysteryType), out float mult))
            return mult;
        return 1.0f;
    }

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
