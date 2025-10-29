using System.Runtime.CompilerServices;
using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

internal class PawnState(MapState mapState, Pawn pawn)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PawnState For(Pawn p)
    {
        return MapState.For(p.MapHeld)[p];
    }

    internal static void Add(Pawn p)
    {
        if (MapState.For(p.MapHeld) is not MapState ms) return;
        ms[p] = new PawnState(ms, p);
    }

    internal static void Remove(Pawn p)
    {
        if (MapState.For(p.MapHeld) is not MapState ms) return;
        ms.Remove(p);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingSunlight(Pawn p)
    {
        if (For(p) is not PawnState s) return false;
        return s.blockingSunlight;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsBlockingWeather(Pawn p)
    {
        if (For(p) is not PawnState s) return false;
        return s.blockingWeather;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaDeployed(Pawn p)
    {
        if (For(p) is not PawnState s) return false;
        return s.blockingSunlight || s.blockingWeather;
    }

    private readonly MapState mapState = mapState;
    private readonly Pawn pawn = pawn;
    private bool blockingSunlight = false;
    private bool blockingWeather = false;

    internal Pawn Pawn
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pawn;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Dirty() => mapState.Dirty(this);

    private bool IsBlockingSunlight(Map map)
    {
        return Mod.Settings.UmbrellasBlockSun
            && map.skyManager.IsUmbrellaSunlight(pawn)
            && !pawn.Position.Roofed(map)
            && pawn.apparel.HasUmbrella();
    }

    private bool IsBlockingWeather(Map map)
    {
        var w = map.weatherManager;
        return w.IsUmbrellaWeather()
            && !pawn.Position.Roofed(map)
            && pawn.apparel.HasUmbrellaOrHatFor(w.UmbrellaWeatherDef());
    }

    internal void Update()
    {
        if (pawn.Dead) return;
        var prev = (blockingSunlight, blockingWeather);
        var map = mapState.Map;
        blockingSunlight = IsBlockingSunlight(map);
        blockingWeather = IsBlockingWeather(map);
        var cur = (blockingSunlight, blockingWeather);
        if (prev == cur) return;
        pawn.UpdateUmbrellaState();
    }
}
