using Bumbershoots.Ext.RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class PawnExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasSunlightSensitivity(this Pawn p)
    {
        return p.genes.HasUVSensitivity();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingSunlight(this Pawn p)
    {
        return PawnState.IsBlockingSunlight(p);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingWeather(this Pawn p)
    {
        return PawnState.IsBlockingWeather(p);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaDeployed(this Pawn p)
    {
        return PawnState.IsUmbrellaDeployed(p);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaState(this Pawn p)
    {
        p.apparel.UpdateUmbrellaState();
        p.health.UpdateUmbrellaState();
    }
}
