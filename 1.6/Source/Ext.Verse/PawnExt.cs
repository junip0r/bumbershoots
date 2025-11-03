using Bumbershoots.Ext.RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class PawnExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PawnState PawnState(this Pawn p)
    {
        return p.GetComp<PawnState>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingSunlight(this Pawn p)
    {
        if (PawnState(p) is not PawnState ps) return false;
        return ps.BlockingSunlight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingWeather(this Pawn p)
    {
        if (PawnState(p) is not PawnState ps) return false;
        return ps.BlockingWeather;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaDeployed(this Pawn p)
    {
        if (PawnState(p) is not PawnState ps) return false;
        return ps.BlockingSunlight || ps.BlockingWeather;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasSunlightSensitivity(this Pawn p)
    {
        return p.genes.HasUVSensitivity();
    }
}
