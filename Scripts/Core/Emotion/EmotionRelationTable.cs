namespace ProjectRA.Core;

public static class EmotionRelationTable
{
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

    public static float GetDisadvantageRate(int levelGap) => GetAdvantageRate(levelGap);
}
