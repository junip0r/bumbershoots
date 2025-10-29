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
        return t.WornApparel.Select(apparel => apparel.def);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrella(this Pawn_ApparelTracker t)
    {
        return GetUmbrella(t) is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrellaHat(this Pawn_ApparelTracker t)
    {
        return GetUmbrellaHat(t) is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrellaOrHat(this Pawn_ApparelTracker t)
    {
        return GetUmbrellaOrHat(t) is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool HasUmbrellaOrHatFor(this Pawn_ApparelTracker t, WeatherDef weather)
    {
        var u = GetUmbrellaOrHat(t);
        return u is not null && u.IsUmbrellaOrHatFor(weather);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrella(this Pawn_ApparelTracker t)
    {
        return WornApparelDefs(t).LastOrDefault(def => def.IsUmbrella());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrellaHat(this Pawn_ApparelTracker t)
    {
        if (Mod.Settings.UmbrellaHats)
        {
            return WornApparelDefs(t).FirstOrDefault(def => def.IsUmbrellaHat());
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ThingDef GetUmbrellaOrHat(this Pawn_ApparelTracker t)
    {
        return WornApparelDefs(t).FirstOrDefault(def => def.IsUmbrellaOrHat());
    }

    internal static void UpdateUmbrellaState(this Pawn_ApparelTracker t)
    {
        t.Notify_ApparelChanged();
        PortraitsCache.SetDirty(t.pawn);
    }
}
