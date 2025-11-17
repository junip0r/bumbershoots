using Bumbershoots.Ext.Verse;
using Bumbershoots.Util;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

public class MapComp(Map map) : MapComponent(map)
{
    private readonly Memo<bool> isUmbrellaSunlight = new(delegate
    {
        return Settings.UmbrellasBlockSun && map.skyManager.CurSkyGlow > 0.1f;
    });

    private readonly Memo<bool> isUmbrellaWeather = new(delegate
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
