using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class GeneDefOfExt
{
    private readonly static LazyDef<GeneDef> uvSensitivity_Mild = new("UVSensitivity_Mild");
    private readonly static LazyDef<GeneDef> uvSensitivity_Intense = new("UVSensitivity_Intense");

    internal static GeneDef UVSensitivity_Mild => uvSensitivity_Mild.Def;
    internal static GeneDef UVSensitivity_Intense => uvSensitivity_Intense.Def;
}
