using Verse;

namespace Bumbershoots.Ext.Verse;

public static partial class Pawn_HealthTrackerExt
{
    public static bool HasUmbrellaProsthetic(this Pawn_HealthTracker @this)
    {
        var count = @this.hediffSet.hediffs.Count;
        for (var i = 0; i < count; i++)
            if (@this.hediffSet.hediffs[i].def.IsUmbrellaProsthetic())
                return true;
        return false;
    }
}
