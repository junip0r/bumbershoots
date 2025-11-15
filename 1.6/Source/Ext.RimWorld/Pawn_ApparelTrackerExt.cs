using RimWorld;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Ext.RimWorld;

internal static class Pawn_ApparelTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaGraphics(this Pawn_ApparelTracker @this)
    {
        @this.Notify_ApparelChanged();
        if (!@this.pawn.IsColonist) return;
        PortraitsCache.SetDirty(@this.pawn);
    }
}
