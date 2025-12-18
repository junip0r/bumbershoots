using Bumbershoots.Ext.RimWorld;
using Prepatcher;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class PawnExt
{
    [PrepatcherField]
    [InjectComponent]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PawnComp PawnComp(this Pawn @this)
    {
        return @this.GetComp<PawnComp>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UmbrellaComp UmbrellaComp(this Pawn @this)
    {
        if (@this.PawnComp() is not PawnComp pawnComp) return null;
        return pawnComp.umbrellaComp;
    }

    public static void Notify_SettingsChanged(this Pawn @this)
    {
        if (@this.PawnComp() is not PawnComp pawnComp) return;
        pawnComp.Notify_SettingsChanged();
    }

    public static bool HasSunlightSensitivity(this Pawn @this)
    {
        return @this.genes.HasUVSensitivity();
    }
}
