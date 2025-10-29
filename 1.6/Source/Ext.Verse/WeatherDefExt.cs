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
    public static bool IsLightRain(this WeatherDef def)
    {
        return LightRain.Any(def.defName.Equals);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHeavyRain(this WeatherDef def)
    {
        return HeavyRain.Any(def.defName.Equals);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRain(this WeatherDef def)
    {
        return IsLightRain(def) || IsHeavyRain(def);
    }
}
