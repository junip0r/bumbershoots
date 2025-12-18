using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class ThingDefExt
{
    internal static readonly HashSet<string> Umbrellas =
    [
        "Bumber_Parasol",
        "Bumber_Umbrella",
        "Bumber_GolfUmbrella",
        "Bumber_FashionUmbrella",
    ];

    internal static bool IsUmbrella(this ThingDef @this)
    {
        return Umbrellas.Contains(@this.defName);
    }
}
