namespace ProjectRA.Cards;

public class CardInstance
{
    public CardData Template { get; }
    public int CurrentCooldown { get; set; }
    public bool IsUpgraded { get; set; }
    public bool IsConsumed { get; set; }
    public bool IsVoid { get; set; }

    public string Id => Template.Id;
    public string Name => Template.Name;
    public string Description => Template.Description;
    public int Cost => IsUpgraded && Template.Upgrade.HasValue
        ? Template.Cost + Template.Upgrade.Value.CostIncrease
        : Template.Cost;
    public CardType Type => Template.Type;
    public CardRarity Rarity => Template.Rarity;
    public CardKeyword Keywords => Template.Keywords;
    public DiceInstance[] Dices => Template.Dices;

    public CardInstance(CardData template)
    {
        Template = template;
        CurrentCooldown = 0;
        IsUpgraded = false;
        IsConsumed = false;
        IsVoid = false;
    }

    public CardInstance Clone()
    {
        return new CardInstance(Template)
        {
            CurrentCooldown = CurrentCooldown,
            IsUpgraded = IsUpgraded,
            IsConsumed = IsConsumed,
            IsVoid = IsVoid,
        };
    }

    public void Upgrade()
    {
        if (!Template.Upgrade.HasValue) return;
        IsUpgraded = true;
    }
}
