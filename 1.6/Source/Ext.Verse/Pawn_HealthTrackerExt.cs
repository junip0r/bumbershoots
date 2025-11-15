using Verse;

namespace Bumbershoots.Ext.Verse;

internal static partial class Pawn_HealthTrackerExt
{
    internal static bool HasUmbrellaProsthetic(this Pawn_HealthTracker @this)
    {
        for (var i = 0; i < @this.hediffSet.hediffs.Count; i++)
        {
            if (@this.hediffSet.hediffs[i].def.IsUmbrellaProsthetic()) return true;
        }
        return false;
    }
}
