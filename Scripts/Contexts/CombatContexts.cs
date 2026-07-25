using ProjectRA.Combat;
using ProjectRA.Entities;

namespace ProjectRA.Contexts;

public enum ModifierRole
{
    Source,
    Target
}

public struct DamageContext
{
    public Creature Target;
    public Creature Dealer;
    public decimal BaseDamage;
    public MysteryType MysteryType;
    public ArmorType TargetArmor;
    public AttackAttribute Attribute;
    public bool IsConfusionDamage;
    public bool IsUnblockable;
    public bool IsFromClash;
    public ModifierRole CurrentRole;
}

public struct ClashContext
{
    public Creature Attacker;
    public Creature Defender;
    public DiceInstance AttackerDice;
    public DiceInstance DefenderDice;
    public int AttackerRoll;
    public int DefenderRoll;
    public bool AttackerWins;
    public bool IsDraw;
}

public struct DiceContext
{
    public Creature Owner;
    public DiceInstance Dice;
    public bool IsDuringClash;
}
