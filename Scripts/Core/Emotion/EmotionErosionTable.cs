using System.Collections.Generic;

namespace ProjectRA.Core;

public struct EmotionErosionData
{
    public EmotionType RequiredEmotion;
    public int RequiredSanity;
    public EmotionType ErosionType;
    public bool RequiresEfwSkill;
    public bool RandomAttack;
}

public static class EmotionErosionTable
{
    public static readonly List<EmotionErosionData> Entries = new()
    {
        new() { RequiredEmotion = EmotionType.Ecstasy, RequiredSanity = +50, ErosionType = EmotionType.Mania,  RequiresEfwSkill = true, RandomAttack = false },
        new() { RequiredEmotion = EmotionType.Fury,    RequiredSanity = -50, ErosionType = EmotionType.Rage,   RequiresEfwSkill = true, RandomAttack = true },
        new() { RequiredEmotion = EmotionType.Depression, RequiredSanity = -50, ErosionType = EmotionType.Despair, RequiresEfwSkill = true, RandomAttack = true },
    };

    public static EmotionErosionData? GetErosion(EmotionType emotion, int sanity)
    {
        foreach (var e in Entries)
            if (e.RequiredEmotion == emotion && sanity == e.RequiredSanity)
                return e;
        return null;
    }
}
