using RimWorld;
using System;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Ext.RimWorld;

internal static class Pawn_GeneTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUVSensitivity(this Pawn_GeneTracker @this)
    {
        if (GeneDefExt.UVSensitivityGeneDefs.Length == 0) return false;
        var genes = @this.GenesListForReading;
        var uvGeneDefs = GeneDefExt.UVSensitivityGeneDefs;
        for (var i = 0; i < genes.Count; i++)
        {
            for (var j = 0; j < uvGeneDefs.Length; j++)
            {
                if (genes[i].def == uvGeneDefs[j] && genes[i].Active) return true;
            }
        }
        return false;
    }
}
