using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using System;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

internal class PawnState(MapState mapState, Pawn pawn)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static PawnState For(Pawn p)
    {
        return MapState.For(p.MapHeld)[p];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Add(Pawn p)
    {
        if (MapState.For(p.MapHeld) is not MapState ms) return;
        ms.Add(new(ms, p));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    internal Map Map
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => mapState.Map;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Dirty() => mapState.Dirty(this);

    internal void Update()
    {
        if (pawn.Dead) return;
        var prev = (blockingSunlight, blockingWeather);
        var map = mapState.Map;
        var wm = map.weatherManager;
        var roofed = new Lazy<bool>(() => pawn.Position.Roofed(map));
        blockingSunlight = Mod.Settings.UmbrellasBlockSun
            && map.skyManager.IsUmbrellaSunlight(pawn)
            && !roofed.Value
            && pawn.apparel.HasUmbrella();
        blockingWeather = wm.IsUmbrellaWeather()
            && !roofed.Value
            && pawn.apparel.HasUmbrellaOrHatFor(wm.UmbrellaWeatherDef());
        var cur = (blockingSunlight, blockingWeather);
        if (prev == cur) return;
        pawn.UpdateUmbrellaState();
    }
}
