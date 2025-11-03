using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class Pawn_ApparelTrackerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<ThingDef> WornApparelDefs(Pawn_ApparelTracker t)
    {
        foreach (var a in t.WornApparel)
        {
            yield return a.def;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsWearingUmbrella(this Pawn_ApparelTracker t)
    {
        return GetUmbrella(t) is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsWearingUmbrellaOrHat(this Pawn_ApparelTracker t)
    {
        return GetUmbrellaOrHat(t) is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsWearingUmbrellaOrHatFor(this Pawn_ApparelTracker t, WeatherDef weather)
    {
        var u = GetUmbrellaOrHat(t);
        return u is not null && u.IsUmbrellaOrHatFor(weather);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrella(this Pawn_ApparelTracker t)
    {
        foreach (var def in WornApparelDefs(t))
        {
            if (def.IsUmbrella())
            {
                return def;
            }
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrellaOrHat(this Pawn_ApparelTracker t)
    {
        foreach (var def in WornApparelDefs(t))
        {
            if (def.IsUmbrellaOrHat())
            {
                return def;
            }
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void UpdateUmbrellaGraphics(this Pawn_ApparelTracker t)
    {
        t.Notify_ApparelChanged();
        PortraitsCache.SetDirty(t.pawn);
    }
}
