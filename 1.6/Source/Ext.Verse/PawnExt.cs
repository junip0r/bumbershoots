using Bumbershoots.Ext.RimWorld;
using RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class PawnExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PawnState PawnState(this Pawn @this)
    {
        return @this.GetComp<PawnState>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingSunlight(this Pawn @this)
    {
        if (@this.PawnState() is not PawnState ps) return false;
        return ps.BlockingSunlight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingWeather(this Pawn @this)
    {
        if (@this.PawnState() is not PawnState ps) return false;
        return ps.BlockingWeather;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaDeployed(this Pawn @this)
    {
        if (@this.PawnState() is not PawnState ps) return false;
        return ps.BlockingSunlight || ps.BlockingWeather;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasSunlightSensitivity(this Pawn @this)
    {
        return @this.genes is not null && @this.genes.HasUVSensitivity();
    }
}
