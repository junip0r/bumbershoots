using RimWorld;

namespace Bumbershoots.Ext.RimWorld;

public static class Pawn_GeneTrackerExt
{
    public static bool HasUVSensitivity(this Pawn_GeneTracker @this)
    {
        var uvGeneDefs = GeneDefExt.UVSensitivityDefs;
        if (uvGeneDefs.Length == 0) return false;
        var genes = @this.GenesListForReading;
        var count = genes.Count;
        for (var i = 0; i < count; i++)
        {
            var length = uvGeneDefs.Length;
            for (var j = 0; j < length; j++)
                if (genes[i].def == uvGeneDefs[j] && genes[i].Active)
                    return true;
        }
        return false;
    }
}
