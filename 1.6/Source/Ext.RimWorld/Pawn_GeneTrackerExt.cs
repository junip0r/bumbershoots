using RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class Pawn_GeneTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUVSensitivity(this Pawn_GeneTracker @this)
    {
        if (!ModsConfig.BiotechActive) return false;
        return @this.HasActiveGene(GeneDefOfExt.UVSensitivity_Mild)
            || @this.HasActiveGene(GeneDefOfExt.UVSensitivity_Intense);
    }
}
