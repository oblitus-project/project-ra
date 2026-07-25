using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectRA.Combat;
using ProjectRA.Contexts;
using ProjectRA.Core;
using ProjectRA.Entities;
using ProjectRA.Hooks;

namespace ProjectRA.Commands;

public static class ClashCmd
{
	public static ClashBuilder Resolve() => new();

	public class ClashBuilder
	{
		private Creature _attacker;
		private Creature _defender;
		private DiceInstance _atkDice;
		private DiceInstance _defDice;
		private int _atkSanity;
		private int _defSanity;
		private bool _useLevelSuppression = true;
		private int _previousRollPower;

		public ClashBuilder WithAttacker(Creature creature, DiceInstance dice)
		{
			_attacker = creature;
			_atkDice = dice;
			return this;
		}

		public ClashBuilder WithDefender(Creature creature, DiceInstance dice)
		{
			_defender = creature;
			_defDice = dice;
			return this;
		}

		public ClashBuilder WithSanity(int attackerSanity, int defenderSanity)
		{
			_atkSanity = attackerSanity;
			_defSanity = defenderSanity;
			return this;
		}

		public ClashBuilder WithLevelSuppression(bool enabled)
		{
			_useLevelSuppression = enabled;
			return this;
		}

		public ClashBuilder WithPreviousRollPower(int previousPower)
		{
			_previousRollPower = previousPower;
			return this;
		}

		public async Task<ClashResult> ExecuteAsync()
		{
			var ctx = new ClashContext
			{
				Attacker = _attacker,
				Defender = _defender,
				AttackerDice = _atkDice,
				DefenderDice = _defDice,
			};

			await Hook.BeforeClash(ctx);

			int atkRoll = RollDiceWithSanity(_atkDice, _atkSanity);
			int defRoll = RollDiceWithSanity(_defDice, _defSanity);

			atkRoll = Hook.ModifyDicePower(_attacker, atkRoll,
				new DiceContext { Owner = _attacker, Dice = _atkDice, IsDuringClash = true });
			defRoll = Hook.ModifyDicePower(_defender, defRoll,
				new DiceContext { Owner = _defender, Dice = _defDice, IsDuringClash = true });

			if (_useLevelSuppression)
			{
				int atkLevel = _attacker.AtkLevel;
				int defLevel = _defender.AtkLevel;
				int diff = atkLevel - defLevel;
				if (diff > 0) atkRoll += diff / 3;
				else if (diff < 0) defRoll += -diff / 3;
			}

			int atkFinal = ApplyOperation(_atkDice, atkRoll);
			int defFinal = ApplyOperation(_defDice, defRoll);

			bool attackerWins = atkRoll > defRoll;
			bool isDraw = atkRoll == defRoll;

			int winFinalPower = attackerWins ? atkFinal : defFinal;
			var winDice = attackerWins ? _atkDice : _defDice;
			var winner = attackerWins ? _attacker : _defender;
			var loser = attackerWins ? _defender : _attacker;

			var clashCtx = new ClashContext
			{
				Attacker = _attacker,
				Defender = _defender,
				AttackerDice = _atkDice,
				DefenderDice = _defDice,
				AttackerRoll = atkRoll,
				DefenderRoll = defRoll,
				AttackerWins = attackerWins,
				IsDraw = isDraw,
			};

			if (!isDraw)
			{
				if (attackerWins)
					await Hook.OnClashWin(winner, clashCtx);
				else
					await Hook.OnClashLose(loser, clashCtx);
			}

			GuardDiceResolver.GuardResult? guardOutcome = null;
			if (DefenseDiceTypes.Contains(_defDice.Type))
			{
				guardOutcome = GuardDiceResolver.Resolve(_defDice.Type, _atkDice.Type);
			}

			return new ClashResult
			{
				Winner = isDraw ? null : winner,
				Loser = isDraw ? null : loser,
				WinnerDice = winDice,
				RollValue = attackerWins ? atkRoll : defRoll,
				FinalPower = winFinalPower,
				IsDraw = isDraw,
				GuardOutcome = guardOutcome,
			};
		}

		private static int RollDiceWithSanity(DiceInstance dice, int sanity)
		{
			int min = dice.BaseMin + dice.LevelMod;
			int max = dice.BaseMax + dice.LevelMod;
			var weights = SanitySystem.CalculateDiceWeights(min, max, sanity);
			int range = weights.EffectiveMax - weights.EffectiveMin + 1;
			float totalWeight = 0f;
			foreach (var w in weights.Weights) totalWeight += w;
			float roll = (float)new Random().NextDouble() * totalWeight;
			float cumulative = 0f;
			for (int i = 0; i < range; i++)
			{
				cumulative += weights.Weights[i];
				if (roll <= cumulative)
					return weights.EffectiveMin + i;
			}
			return weights.EffectiveMax;
		}

		private int ApplyOperation(DiceInstance dice, int rollValue)
		{
			if (dice.Operation == null)
				return rollValue;

			var op = dice.Operation.Value;
			int baseValue = op.UsePreviousAsBase ? _previousRollPower : rollValue;
			int varValue = op.Variation.Roll(new Random());
			return op.Op switch
			{
				OperationType.Add => baseValue + varValue,
				OperationType.Subtract => baseValue - varValue,
				OperationType.Multiply => baseValue * varValue,
				OperationType.Divide => varValue != 0 ? baseValue / varValue : baseValue,
				_ => baseValue,
			};
		}
	}

	private static readonly HashSet<DiceType> DefenseDiceTypes = new()
	{
		DiceType.Defensive,
		DiceType.Block,
		DiceType.Evade,
	};
}
