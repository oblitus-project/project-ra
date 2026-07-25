using ProjectRA.Combat;
using ProjectRA.Entities;

namespace ProjectRA.Commands;

public struct ClashResult
{
    public Creature Winner;
    public Creature Loser;
    public DiceInstance WinnerDice;
    public int RollValue;
    public int FinalPower;
    public bool IsDraw;
    public GuardDiceResolver.GuardResult? GuardOutcome;
}
