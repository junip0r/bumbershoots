using RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class Pawn_GeneTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUVSensitivity(this Pawn_GeneTracker genes)
    {
        if (!ModsConfig.BiotechActive) return false;
        return genes.HasActiveGene(GeneDefOfExt.UVSensitivity_Mild)
            || genes.HasActiveGene(GeneDefOfExt.UVSensitivity_Intense);
    }
}
