using System;
using System.Linq;
using ProjectRA.Core;

namespace ProjectRA.Combat;

public class ClashResolver
{
    public struct ClashResult
    {
        public int PlayerRoll;
        public int EnemyRoll;
        public bool PlayerWins;
        public bool IsDraw;
    }

    public static ClashResult ResolveClash(
        DiceInstance playerDice, int playerAtkLevel,
        DiceInstance enemyDice, int enemyAtkLevel,
        int playerSanity, int enemySanity, Random rng)
    {
        int pRoll = RollDiceWithSanity(playerDice, playerSanity, rng);
        int eRoll = RollDiceWithSanity(enemyDice, enemySanity, rng);

        int levelDiff = playerAtkLevel - enemyAtkLevel;
        if (levelDiff > 0)
        {
            int bonus = levelDiff / 3;
            pRoll += bonus;
        }
        else if (levelDiff < 0)
        {
            int penalty = -levelDiff / 3;
            eRoll += penalty;
        }

        return new ClashResult
        {
            PlayerRoll = pRoll,
            EnemyRoll = eRoll,
            PlayerWins = pRoll > eRoll,
            IsDraw = pRoll == eRoll
        };
    }

    private static int RollDiceWithSanity(DiceInstance dice, int sanity, Random rng)
    {
        var weights = SanitySystem.CalculateDiceWeights(dice.BaseMin, dice.BaseMax, sanity);
        int range = weights.EffectiveMax - weights.EffectiveMin + 1;
        float totalWeight = weights.Weights.Sum();
        float roll = (float)rng.NextDouble() * totalWeight;
        float cumulative = 0;
        for (int i = 0; i < range; i++)
        {
            cumulative += weights.Weights[i];
            if (roll <= cumulative)
                return weights.EffectiveMin + i;
        }
        return weights.EffectiveMax;
    }
}
