using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class Pawn_ApparelTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<ThingDef> WornApparelDefs(this Pawn_ApparelTracker @this)
    {
        foreach (var a in @this.WornApparel)
        {
            yield return a.def;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsWearingUmbrella(this Pawn_ApparelTracker @this)
    {
        return @this.GetUmbrella() is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsWearingUmbrellaOrHat(this Pawn_ApparelTracker @this)
    {
        return @this.GetUmbrellaOrHat() is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsWearingUmbrellaOrHatFor(this Pawn_ApparelTracker @this, WeatherDef weather)
    {
        var u = @this.GetUmbrellaOrHat();
        return u is not null && u.IsUmbrellaOrHatFor(weather);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrella(this Pawn_ApparelTracker @this)
    {
        foreach (var def in @this.WornApparelDefs())
        {
            if (def.IsUmbrella())
            {
                return def;
            }
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrellaOrHat(this Pawn_ApparelTracker @this)
    {
        foreach (var def in @this.WornApparelDefs())
        {
            if (def.IsUmbrellaOrHat())
            {
                return def;
            }
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaGraphics(this Pawn_ApparelTracker @this)
    {
        @this.Notify_ApparelChanged();
        PortraitsCache.SetDirty(@this.pawn);
    }
}
