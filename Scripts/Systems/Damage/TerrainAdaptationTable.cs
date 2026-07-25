using System.Collections.Generic;

namespace ProjectRA.Combat;

public struct TerrainAdaptationRow
{
    public char Grade;
    public float CoverTransferRate;
    public float SelfDamageRate;
}

public static class TerrainAdaptationTable
{
    private static readonly Dictionary<char, TerrainAdaptationRow> Rows = new()
    {
        { 'D', new TerrainAdaptationRow { Grade = 'D', CoverTransferRate = 0.00f, SelfDamageRate = -0.20f } },
        { 'C', new TerrainAdaptationRow { Grade = 'C', CoverTransferRate = 0.10f, SelfDamageRate = -0.10f } },
        { 'B', new TerrainAdaptationRow { Grade = 'B', CoverTransferRate = 0.20f, SelfDamageRate = 0.00f } },
        { 'A', new TerrainAdaptationRow { Grade = 'A', CoverTransferRate = 0.30f, SelfDamageRate = 0.10f } },
        { 'S', new TerrainAdaptationRow { Grade = 'S', CoverTransferRate = 0.40f, SelfDamageRate = 0.20f } },
        { 'R', new TerrainAdaptationRow { Grade = 'R', CoverTransferRate = 0.50f, SelfDamageRate = 0.30f } },
    };

    public static TerrainAdaptationRow? Get(char grade) =>
        Rows.TryGetValue(grade, out var row) ? row : null;
}
