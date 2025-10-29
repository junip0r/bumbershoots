using Bumbershoots.Ext.RimWorld;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class PawnExt
{
    internal static bool HasSunlightSensitivity(this Pawn p)
    {
        return p.genes.HasUVSensitivity();
    }

    internal static void UpdateUmbrellaState(this Pawn p)
    {
        p.apparel.UpdateUmbrellaState();
        p.health.UpdateUmbrellaState();
    }
}
