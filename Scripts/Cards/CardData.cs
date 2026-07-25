using ProjectRA.Combat;

namespace ProjectRA.Cards;

public enum CardRarity
{
    Normal,
    Uncommon,
    Rare,
    Special
}

public enum SinAttribute
{
    None,
    Lust,
    Gluttony,
    Envy,
    Wrath,
    Pride,
    Melancholy,
    Sloth
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
    public int Cost;
    public int SanityCost;
    public int Cooldown;
    public int Copies;
    public int Level;
    public CardType Type;
    public CardRarity Rarity;
    public SinAttribute Sin;
    public CardKeyword Keywords;
    public DiceInstance[] Dices;
    public CardEffectEntry[] Effects;
    public CardUpgrade? Upgrade;
    public string ExtraCostLabelKey;

    public string Name => LocalizationManager.Get(NameKey);
    public string Description => LocalizationManager.Get(DescKey);
    public string ExtraCostLabel => string.IsNullOrEmpty(ExtraCostLabelKey)
        ? "" : LocalizationManager.Get(ExtraCostLabelKey);
}
