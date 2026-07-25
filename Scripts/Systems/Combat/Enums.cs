namespace ProjectRA.Combat;

public enum AttackAttribute { Slash, Pierce, Blunt }
public enum AttackRange { Melee, Ranged, MassIndividual, MassSummation, MassSplash }
public enum MysteryType { Explosive, Piercing, Corrosive, Mystic, Sonic, Normal }
public enum ArmorType { LightArmor, HeavyArmor, CompositeArmor, SpecialArmor, ElasticArmor, NormalArmor, Structure }
public enum CardType { AttackCard, GuardCard, SupportCard, SkillCard, AbilityCard }
public enum DiceType { Offensive, Defensive, Block, Evade, Heal, MassAttack, MassHeal }
public enum DiceSubType { Normal, Counter, Anchor, Derived }
public enum OperationType { Add, Subtract, Multiply, Divide }

public enum EffectTiming
{
    BattleStart, TurnStart, TurnEnd, CombatOpen, BattleEnd,
    OnUse, WhileUsing, OnEquip, WhileEquipped,
    OnClash, OnClashWin, OnClashLose,
    BeforeAttack, AfterAttack, OnHit,
    OnHitUndestroyed, OnHitDestroyed,
    OnDamageTaken, AfterDamageTaken, OnEvade,
    OnEmotionGain, OnConfusion, OnDown, OnKill,
    OnDiscard, OnDraw
}

public enum BonusLayer
{
    Terrain_LevelSuppression,
    MysteryAffinity,
    DamageResistance,
    DamageRate_TakenRate_Proficiency,
    CritHit,
    DealDamage_TakeDamage
}
