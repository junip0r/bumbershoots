using System.Collections.Generic;
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

    public static bool IsUmbrella(this ThingDef @this)
    {
        return Umbrellas.Contains(@this.defName);
    }
}
