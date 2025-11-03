using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

public partial class PawnState : ThingComp
{
    private bool blockingSunlight;
    private bool blockingWeather;
    private int prevState;

    internal bool BlockingSunlight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => blockingSunlight;
    }

    internal bool BlockingWeather
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => blockingWeather;
    }

    public override void Initialize(CompProperties props)
    {
        base.Initialize(props);
#if DEBUG_PAWNSTATE
        I(null, nameof(Initialize), $"init pawn state: {parent}");
#endif
        prevState = State(null);
    }

#if DEBUG_PAWNSTATE
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void I(Map map, string method, string msg)
    {
        Log.I(map, $"# {typeof(PawnState)}.{method} # {msg}");
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int State(Map map) =>
        (
            map,
            blockingSunlight,
            blockingWeather
        ).GetHashCode();

    internal void Update(MapState mapState)
    {
        var curState = prevState;
        try
        {
#if DEBUG_PAWNSTATE
            I(mapState.map, nameof(Update), $"update pawn state: {parent}");
#endif
            var pawn = (Pawn)parent;
            if (pawn.Dead) return;
            var map = mapState.map;
            var wm = map.weatherManager;
            var roofed = pawn.Position.Roofed(map);
            blockingSunlight = Mod.Settings.UmbrellasBlockSun
                && map.skyManager.IsUmbrellaSunlight(pawn)
                && !roofed
                && pawn.apparel.IsWearingUmbrella();
            blockingWeather = wm.IsUmbrellaWeather()
                && !roofed
                && wm.UmbrellaWeatherDef() is WeatherDef weather
                && pawn.apparel.IsWearingUmbrellaOrHatFor(weather);
            curState = State(map);
            if (curState == prevState) return;
            pawn.apparel.UpdateUmbrellaGraphics();
            pawn.health.UpdateUmbrellaHediffs();
        }
        finally
        {
            prevState = curState;
        }
    }

    internal void Clear()
    {
        blockingSunlight = false;
        blockingWeather = false;
        prevState = State(null);
    }
}
