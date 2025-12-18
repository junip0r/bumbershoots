using Verse;

namespace Bumbershoots.Ext.Verse;

public static partial class Pawn_HealthTrackerExt
{
    public static bool HasUmbrellaProsthetic(this Pawn_HealthTracker @this)
    {
        for (var i = 0; i < @this.hediffSet.hediffs.Count; i++)
            if (@this.hediffSet.hediffs[i].def.IsUmbrellaProsthetic())
                return true;
        return false;
    }
}
