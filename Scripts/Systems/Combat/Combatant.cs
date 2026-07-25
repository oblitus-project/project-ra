using System;
using System.Collections.Generic;
using ProjectRA.Cards;
using ProjectRA.Core;
using ProjectRA.Entities;

namespace ProjectRA.Combat;

public class Combatant
{
    public Creature Creature { get; }
    public Deck Deck { get; }
    public SpeedDicePool SpeedDicePool { get; }
    public CostSystem CostSystem { get; }
    public int EmotionLevel { get; set; }
    public int AccumulatedEmotion { get; set; }
    public EmotionState Emotion { get; set; }
    public bool IsPlayerControlled { get; set; }

    public int SpeedMin { get; set; }
    public int SpeedMax { get; set; }

    public bool IsStaggered
    {
        get => Creature.IsStaggered;
        set => Creature.IsStaggered = value;
    }

    public int Hp
    {
        get => Creature.Hp;
        set => Creature.Hp = value;
    }

    public int MaxHp => Creature.MaxHp;
    public int StaggerResistance
    {
        get => Creature.StaggerResistance;
        set => Creature.StaggerResistance = value;
    }

    public int MaxStaggerResistance => Creature.MaxStaggerResistance;

    public int Sanity
    {
        get => Creature.Sanity;
        set => Creature.Sanity = Math.Clamp(value, SanitySystem.MinSanity, SanitySystem.MaxSanity);
    }

    public bool IsAlive => Creature.Hp > 0;
    public bool IsDowned => Creature.Hp <= 0;

    public Combatant(Creature creature, IEnumerable<CardData> deckCards,
        int speedMin, int speedMax, bool isPlayerControlled)
    {
        Creature = creature;
        Deck = new Deck(deckCards, new Random());
        SpeedDicePool = new SpeedDicePool();
        CostSystem = new CostSystem();
        SpeedMin = speedMin;
        SpeedMax = speedMax;
        IsPlayerControlled = isPlayerControlled;
        EmotionLevel = 0;
        AccumulatedEmotion = 0;
        Emotion = default;
        creature.Deck = Deck;
        creature.Sanity = 0;
    }

    public void Initialize()
    {
        Deck.Initialize();
        var initialHand = Deck.Draw(5);
        SpeedDicePool.Reset();
    }

    public void OnTurnStart()
    {
        Deck.ResetCooldowns();
        Deck.TickCooldowns();

        var row = EmotionLevelTable.Get(EmotionLevel);
        SpeedDicePool.Roll(row.SpeedDiceCount, SpeedMin + row.SpeedBonus,
            SpeedMax + row.SpeedBonus, new Random());

        CostSystem.OnTurnStart();
        var drawCount = row.DrawCount;
        Deck.Draw(drawCount);
    }

    public bool CanLevelUpEmotion()
    {
        return EmotionLevelTable.CanLevelUp(EmotionLevel, AccumulatedEmotion);
    }

    public void TryLevelUpEmotion()
    {
        if (!CanLevelUpEmotion()) return;
        AccumulatedEmotion = 0;
        EmotionLevel++;
        CostSystem.OnEmotionLevelUp();
    }

    public void AddEmotion(int amount)
    {
        AccumulatedEmotion += amount;
    }

    public void OnTurnEnd()
    {
        Deck.OnTurnEnd();
    }

    public CardInstance[] GetEquippableCards()
    {
        var available = new List<CardInstance>();
        foreach (var card in Deck.Hand)
        {
            if (card.Keywords.HasFlag(CardKeyword.CannotPlay)) continue;
            if (card.Type == CardType.SkillCard || card.Type == CardType.AbilityCard) continue;
            if (card.CurrentCooldown > 0) continue;
            available.Add(card);
        }
        return available.ToArray();
    }

    public CardInstance[] GetPlayableSkills()
    {
        var skills = new List<CardInstance>();
        foreach (var card in Deck.Hand)
        {
            if (card.Keywords.HasFlag(CardKeyword.CannotPlay)) continue;
            if (card.Type != CardType.SkillCard && card.Type != CardType.AbilityCard) continue;
            if (card.CurrentCooldown > 0) continue;
            skills.Add(card);
        }
        return skills.ToArray();
    }

    public bool CanPlayCard(CardInstance card)
    {
        if (card.Keywords.HasFlag(CardKeyword.CannotPlay)) return false;
        if (card.CurrentCooldown > 0) return false;
        return CostSystem.CurrentCost >= card.Cost;
    }
}
