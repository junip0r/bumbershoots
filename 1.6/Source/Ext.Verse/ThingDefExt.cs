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
    public static bool IsUmbrella(this ThingDef @this)
    {
        return Umbrellas.Contains(@this.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHeavyUmbrella(this ThingDef @this)
    {
        return HeavyUmbrellas.Contains(@this.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSpecialUmbrella(this ThingDef @this)
    {
        return SpecialUmbrellas.Contains(@this.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaHat(this ThingDef @this)
    {
        return Mod.Settings.UmbrellaHats && UmbrellaHats.Contains(@this.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaOrHat(this ThingDef @this)
    {
        return @this.IsUmbrella() || @this.IsUmbrellaHat();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaOrHatFor(this ThingDef @this, WeatherDef weather)
    {
        if (weather.IsLightRain()) return @this.IsUmbrellaOrHat();
        if (weather.IsHeavyRain()) return @this.IsHeavyUmbrella();
        return false;
    }
}
