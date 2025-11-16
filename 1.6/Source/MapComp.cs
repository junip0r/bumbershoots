using Bumbershoots.Ext.Verse;
using Bumbershoots.Util;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    // IsUmbrellaSunlight and IsUmbrellaWeather help UmbrellaComp determine
    // whether it's safe to skip doing a bit of work each tick.
    //
    // IsUmbrellaSunlight lets UmbrellaComp skip the sunlight checks at night
    // for unroofed pawns.
    //
    // IsUmbrellaWeather lets UmbrellaComp skip the weather checks during
    // not-rainy weather for unroofed pawns.

    private readonly LazyMut<bool> isUmbrellaSunlight = new(delegate
    {
        return Settings.UmbrellasBlockSun && map.skyManager.CurSkyGlow > 0.1f;
    });

    private readonly LazyMut<bool> isUmbrellaWeather = new(delegate
    {
        var weather = map.weatherManager.CurWeatherLerped;
        return weather.rainRate > 0 && !weather.IsSnow();
    });

    internal bool IsUmbrellaSunlight
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => isUmbrellaSunlight.Value;
    }

    internal bool IsUmbrellaWeather
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => isUmbrellaWeather.Value;
    }

    public override void MapComponentTick()
    {
        isUmbrellaSunlight.Clear();
        isUmbrellaWeather.Clear();
    }
}
