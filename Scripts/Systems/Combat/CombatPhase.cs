namespace ProjectRA.Combat;

public enum CombatPhase
{
    Intro,
    Preparation,
    PlayerAction,
    Battle,
    TurnEnd,
    Victory,
    Defeat
}

public enum TurnStep
{
    NotStarted,
    PreBattleTriggers,
    RollSpeedDice,
    RecoverCostAndDraw,
    EmotionLevelUp,
    PlayerEquipCards,
    EnemyAssignCards,
    BattleResolve,
    PostBattleTriggers,
    DiscardPhase,
    Complete
}
