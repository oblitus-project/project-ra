using System.Collections.Generic;
using System.Linq;
using ProjectRA.Cards;
using ProjectRA.Combat;
using ProjectRA.Keyword;
using ProjectRA.Models;

namespace ProjectRA.Entities;

public class Creature
{
    public List<StatusEffectModel> ActiveEffects { get; } = new();

    public Deck Deck { get; set; }
    public IReadOnlyList<CardInstance> Hand => Deck?.Hand ?? emptyHand;
    private static readonly List<CardInstance> emptyHand = new();

    public int Hp;
    public int MaxHp;
    public int StaggerResistance;
    public int MaxStaggerResistance;
    public bool IsStaggered;
    public int Sanity;

    public int AtkLevel;
    public int DefLevel;
    public ArmorType ArmorType;

    public void AddEffect(StatusEffectModel effect)
    {
        effect.Owner = this;

        foreach (var existing in ActiveEffects)
        {
            if (existing.Id == effect.Id)
            {
                bool isIndestructible = effect.Category.HasFlag(Keyword.EffectCategory.Indestructible);
                if (!isIndestructible)
                    existing.AddStack(effect.Stack);
                return;
            }
        }

        ActiveEffects.Add(effect);
    }

    public void RemoveEffect(ModelId id)
    {
        ActiveEffects.RemoveAll(e => e.Id == id);
    }

    public void RemoveEffect(string entryName)
    {
        ActiveEffects.RemoveAll(e => e.Id.Entry == entryName);
    }

    public void ClearEffects()
    {
        ActiveEffects.Clear();
    }

    public T GetEffect<T>() where T : StatusEffectModel
    {
        return ActiveEffects.OfType<T>().FirstOrDefault();
    }

    public bool HasEffect(ModelId id)
    {
        return ActiveEffects.Any(e => e.Id == id);
    }
}
