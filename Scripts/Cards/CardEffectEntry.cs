using ProjectRA.Combat;

namespace ProjectRA.Cards;

public struct CardEffectEntry
{
    public EffectTiming Timing;
    public string DescriptionKey;
    public string[] Args;

    public CardEffectEntry(EffectTiming timing, string descriptionKey, params string[] args)
    {
        Timing = timing;
        DescriptionKey = descriptionKey;
        Args = args ?? System.Array.Empty<string>();
    }
}
