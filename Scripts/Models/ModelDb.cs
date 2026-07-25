using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectRA.Models;

public static class ModelDb
{
    private static readonly Dictionary<ModelId, AbstractModel> _models = new();
    private static bool _initialized;

    private static readonly Dictionary<Type, ModelId> _typeToId = new();

    public static IReadOnlyDictionary<ModelId, AbstractModel> All => _models;

    public static void Init()
    {
        if (_initialized) return;

        var modelTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(AbstractModel)))
            .OrderBy(t => t.FullName);

        foreach (var type in modelTypes)
        {
            var instance = (AbstractModel)Activator.CreateInstance(type, nonPublic: true);
            _models[instance.Id] = instance;
            _typeToId[type] = instance.Id;
        }

        _initialized = true;
    }

    public static ModelId GenerateId(Type type)
    {
        string category = GetCategory(type);
        string entry = GetEntry(type);
        return new ModelId(category, entry);
    }

    private static string GetCategory(Type type)
    {
        var ns = type.Namespace ?? "";
        var parts = ns.Split('.');
        return parts.Length >= 2 ? parts[^1] : type.Name;
    }

    private static string GetEntry(Type type)
    {
        var name = type.Name;
        if (name.EndsWith("Effect"))
            name = name.Substring(0, name.Length - "Effect".Length);
        return name.ToLowerInvariant();
    }

    public static T GetCanonical<T>(ModelId id) where T : AbstractModel
        => (T)_models[id];

    public static T CreateMutable<T>(ModelId id) where T : AbstractModel
        => (T)GetCanonical<T>(id).ToMutable();

    public static T CreateMutable<T>() where T : AbstractModel
        => (T)GetCanonical<T>(_typeToId[typeof(T)]).ToMutable();
}
