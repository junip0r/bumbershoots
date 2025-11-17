using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class WeatherDefExt
{
    internal static bool IsSnow(this WeatherDef @this)
    {
        return @this.defName.ToLowerInvariant().Contains("snow");
    }

    internal static bool IsRain(this WeatherDef @this)
    {
        return @this.rainRate > 0 && !@this.IsSnow();
    }
}
