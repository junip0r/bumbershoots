using Bumbershoots.Ext.RimWorld;

namespace Bumbershoots.Reactor.Map;

internal class WeatherMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    private bool? prev, cur = null;

    protected override void OnTick(int _)
    {
        prev = cur;
        cur = mapState.Map.weatherManager.IsUmbrellaWeather();
        if (cur != prev) mapState.Dirty();
    }

    protected override void OnReset() => prev = cur = null;
}
