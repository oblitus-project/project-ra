using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRA.Cards;

public class Deck
{
    private readonly List<CardData> _masterList;
    private readonly List<CardInstance> _drawPile;
    private readonly List<CardInstance> _discardPile;
    private readonly List<CardInstance> _exhaustPile;
    private readonly List<CardInstance> _hand;
    private readonly Random _rng;

    public IReadOnlyList<CardInstance> DrawPile => _drawPile;
    public IReadOnlyList<CardInstance> DiscardPile => _discardPile;
    public IReadOnlyList<CardInstance> ExhaustPile => _exhaustPile;
    public IReadOnlyList<CardInstance> Hand => _hand;
    public IReadOnlyList<CardData> MasterList => _masterList;

    public int DrawPileCount => _drawPile.Count;
    public int DiscardPileCount => _discardPile.Count;
    public int HandCount => _hand.Count;

    public event Action OnDeckShuffled;
    public event Action<CardInstance> OnCardDrawn;
    public event Action<CardInstance> OnCardDiscarded;
    public event Action<CardInstance> OnCardExhausted;

    public Deck(IEnumerable<CardData> masterList, Random rng = null)
    {
        _masterList = masterList.ToList();
        _drawPile = new List<CardInstance>();
        _discardPile = new List<CardInstance>();
        _exhaustPile = new List<CardInstance>();
        _hand = new List<CardInstance>();
        _rng = rng ?? new Random();
    }

    public void Initialize()
    {
        _drawPile.Clear();
        _discardPile.Clear();
        _exhaustPile.Clear();
        _hand.Clear();

        foreach (var cardData in _masterList)
        {
            for (int i = 0; i < cardData.Copies; i++)
            {
                _drawPile.Add(new CardInstance(cardData));
            }
        }

        Shuffle(_drawPile);
    }

    public CardInstance Draw()
    {
        if (_drawPile.Count == 0)
        {
            if (_discardPile.Count == 0) return null;
            ShuffleDiscardIntoDraw();
        }

        var card = _drawPile[0];
        _drawPile.RemoveAt(0);
        _hand.Add(card);
        OnCardDrawn?.Invoke(card);
        return card;
    }

    public CardInstance[] Draw(int count)
    {
        var drawn = new List<CardInstance>();
        for (int i = 0; i < count; i++)
        {
            var card = Draw();
            if (card == null) break;
            drawn.Add(card);
        }
        return drawn.ToArray();
    }

    public void Discard(CardInstance card)
    {
        if (!_hand.Remove(card)) return;
        _discardPile.Add(card);
        OnCardDiscarded?.Invoke(card);
    }

    public void DiscardAll()
    {
        var cards = _hand.ToArray();
        foreach (var card in cards)
            Discard(card);
    }

    public void DiscardAt(int index)
    {
        if (index < 0 || index >= _hand.Count) return;
        Discard(_hand[index]);
    }

    public void Exhaust(CardInstance card)
    {
        if (_hand.Remove(card))
        {
            card.IsConsumed = true;
            _exhaustPile.Add(card);
            OnCardExhausted?.Invoke(card);
            return;
        }

        if (_drawPile.Remove(card))
        {
            card.IsConsumed = true;
            _exhaustPile.Add(card);
            return;
        }

        if (_discardPile.Remove(card))
        {
            card.IsConsumed = true;
            _exhaustPile.Add(card);
            return;
        }
    }

    public void MoveFromHandToDraw(CardInstance card)
    {
        if (!_hand.Remove(card)) return;
        _drawPile.Insert(0, card);
    }

    public void MoveFromDiscardToHand(CardInstance card)
    {
        if (!_discardPile.Remove(card)) return;
        _hand.Add(card);
    }

    public void ShuffleDiscardIntoDraw()
    {
        foreach (var card in _discardPile)
            _drawPile.Add(card);
        _discardPile.Clear();
        Shuffle(_drawPile);
    }

    public bool HasCardInHand(string cardId)
    {
        return _hand.Any(c => c.Id == cardId);
    }

    public CardInstance FindInHand(string cardId)
    {
        return _hand.FirstOrDefault(c => c.Id == cardId);
    }

    public CardInstance[] FindAllInHand(string cardId)
    {
        return _hand.Where(c => c.Id == cardId).ToArray();
    }

    public void OnTurnEnd()
    {
        var toDiscard = _hand.Where(c =>
            !c.Keywords.HasFlag(CardKeyword.Retain) &&
            !c.Keywords.HasFlag(CardKeyword.Eternal)).ToArray();
        foreach (var card in toDiscard)
            Discard(card);

        var voidCards = _hand.Where(c =>
            c.Keywords.HasFlag(CardKeyword.Void) &&
            !c.IsVoid).ToArray();
        foreach (var card in voidCards)
        {
            card.IsVoid = true;
            Exhaust(card);
        }
    }

    public void ResetCooldowns()
    {
        foreach (var card in _drawPile)
            card.CurrentCooldown = 0;
        foreach (var card in _discardPile)
            card.CurrentCooldown = 0;
        foreach (var card in _hand)
            card.CurrentCooldown = 0;
    }

    public void TickCooldowns()
    {
        foreach (var card in _drawPile)
            if (card.CurrentCooldown > 0) card.CurrentCooldown--;
        foreach (var card in _discardPile)
            if (card.CurrentCooldown > 0) card.CurrentCooldown--;
        foreach (var card in _hand)
            if (card.CurrentCooldown > 0) card.CurrentCooldown--;
    }

    private void Shuffle(List<CardInstance> pile)
    {
        int n = pile.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            (pile[k], pile[n]) = (pile[n], pile[k]);
        }
        OnDeckShuffled?.Invoke();
    }
}
