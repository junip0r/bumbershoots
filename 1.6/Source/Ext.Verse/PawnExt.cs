using System.Runtime.CompilerServices;
using Bumbershoots.Ext.RimWorld;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class PawnExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PawnComp PawnComp(this Pawn @this)
    {
        return @this.GetComp<PawnComp>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static UmbrellaComp UmbrellaComp(this Pawn @this)
    {
        if (@this.PawnComp() is not PawnComp pawnComp) return null;
        return pawnComp.umbrellaComp;
    }

    internal static void Notify_SettingsChanged(this Pawn @this)
    {
        if (@this.PawnComp() is not PawnComp pawnComp) return;
        pawnComp.Notify_SettingsChanged();
    }

    internal static bool HasSunlightSensitivity(this Pawn @this)
    {
        return @this.genes.HasUVSensitivity();
    }
}
