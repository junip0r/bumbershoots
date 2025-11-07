using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class WeatherDefExt
{
    public static readonly HashSet<string> LightRain =
    [
        // Vanilla
        "Rain",
        "FoggyRain",
    ];

    public static readonly HashSet<string> HeavyRain =
    [
        // Vanilla
        "RainyThunderstorm",

        // Odyssey
        "TorrentialRain",
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLightRain(this WeatherDef @this)
    {
        return LightRain.Any(@this.defName.Equals);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHeavyRain(this WeatherDef @this)
    {
        return HeavyRain.Any(@this.defName.Equals);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRain(this WeatherDef @this)
    {
        return @this.IsLightRain() || @this.IsHeavyRain();
    }
}
