using Bumbershoots.Ext.RimWorld;

namespace Bumbershoots.Reactor.Map;

internal class WeatherMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    private bool? prev, cur = null;

    protected override void DoTick(int _)
    {
        prev = cur;
        cur = mapState.Map.weatherManager.IsUmbrellaWeather();
        if (!prev.HasValue) return;
        if (cur != prev) mapState.Dirty();
    }

    protected override void DoReset() => prev = cur = null;
}
