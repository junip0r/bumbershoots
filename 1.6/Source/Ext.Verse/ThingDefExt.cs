using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class ThingDefExt
{
    public static readonly HashSet<string> Umbrellas =
    [
        "Bumber_Parasol",
        "Bumber_Umbrella",
        "Bumber_GolfUmbrella",
        "Bumber_FashionUmbrella",
    ];

    public static readonly HashSet<string> LightUmbrellas =
    [
        "Bumber_Parasol",
    ];

    public static readonly HashSet<string> HeavyUmbrellas =
    [
        "Bumber_Umbrella",
        "Bumber_GolfUmbrella",
        "Bumber_FashionUmbrella",
    ];

    public static readonly HashSet<string> SpecialUmbrellas =
    [
        "Bumber_GolfUmbrella",
        "Bumber_FashionUmbrella",
    ];

    public static readonly HashSet<string> UmbrellaHats =
    [
        "Apparel_CowboyHat",
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrella(this ThingDef t)
    {
        return Umbrellas.Contains(t.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLightUmbrella(this ThingDef t)
    {
        return LightUmbrellas.Contains(t.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHeavyUmbrella(this ThingDef t)
    {
        return HeavyUmbrellas.Contains(t.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSpecialUmbrella(this ThingDef t)
    {
        return SpecialUmbrellas.Contains(t.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaHat(this ThingDef t)
    {
        return Mod.Settings.UmbrellaHats && UmbrellaHats.Contains(t.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaOrHat(this ThingDef t)
    {
        return IsUmbrella(t) || IsUmbrellaHat(t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaOrHatFor(this ThingDef t, WeatherDef weather)
    {
        if (weather.IsLightRain()) return IsUmbrellaOrHat(t);
        if (weather.IsHeavyRain()) return IsHeavyUmbrella(t);
        return false;
    }
}
