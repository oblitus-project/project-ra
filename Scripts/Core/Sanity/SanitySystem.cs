using System;

namespace ProjectRA.Core;

public static class SanitySystem
{
    public const int MinSanity = -50;
    public const int MaxSanity = 50;
    public const float BalanceConstantR = 4f;

    public struct DiceWeightResult
    {
        public int EffectiveMin;
        public int EffectiveMax;
        public float[] Weights;
    }

    public static DiceWeightResult CalculateDiceWeights(int min, int max, int sanity)
    {
        int n = max - min + 1;
        int k = (int)Math.Floor(Math.Abs(sanity) / 50.0 * n / BalanceConstantR);

        int effMin = sanity >= 0 ? min + k : min;
        int effMax = sanity >= 0 ? max : max - k;

        if (effMin > effMax)
        {
            int mid = (min + max) / 2;
            effMin = effMax = mid;
        }

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

    public static int SanityOnClashWin => 5;
    public static int SanityOnClashLose => -2;

    public static int SanityOnConfusion(int emotionLevel) =>
        emotionLevel switch { 1 => -3, 2 => -5, 3 => -10, _ => -3 };

    public static int SanityOnKill(int level, int targetLevel) =>
        15 + (level - targetLevel) / 3;

    public static int SanityOnAllyDown(int level, int targetLevel) =>
        -(15 + (level - targetLevel) / 3);
}
