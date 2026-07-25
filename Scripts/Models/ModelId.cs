using System;

namespace ProjectRA.Models;

public record ModelId(string Category, string Entry) : IComparable<ModelId>
{
    public override string ToString() => $"{Category}.{Entry}";

    public static ModelId Parse(string s)
    {
        var parts = s.Split('.');
        if (parts.Length != 2)
            throw new FormatException($"Invalid ModelId format: '{s}'. Expected 'Category.Entry'");
        return new ModelId(parts[0], parts[1]);
    }

    public int CompareTo(ModelId? other)
    {
        if (other is null) return 1;
        int cat = string.Compare(Category, other.Category, StringComparison.Ordinal);
        return cat != 0 ? cat : string.Compare(Entry, other.Entry, StringComparison.Ordinal);
    }
}
