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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrella(this ThingDef @this)
    {
        return Umbrellas.Contains(@this.defName);
    }
}
