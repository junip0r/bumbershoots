using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class GeneDefOfExt
{
    private readonly static LazyDef<GeneDef> uvSensitivity_Mild = new("UVSensitivity_Mild");
    private readonly static LazyDef<GeneDef> uvSensitivity_Intense = new("UVSensitivity_Intense");

    internal static GeneDef UVSensitivity_Mild
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => uvSensitivity_Mild.Def;
    }

    internal static GeneDef UVSensitivity_Intense
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => uvSensitivity_Intense.Def;
    }
}
