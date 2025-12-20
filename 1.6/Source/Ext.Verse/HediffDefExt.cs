using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class HediffDefExt
{
    public static readonly HashSet<string> UmbrellaProsthetics =
    [
        // FIXME figure out where these hediffs come from

        "AdvancedBionicArm",
        "AdvancedPowerArm",
        "ArchotechArm",
        "Arm",
        "BionicArm",
        "PowerArm",
        "SNS_Hediff_BionicArm_GenI",
        "SNS_Hediff_BionicArm_GenII",
        "SNS_Hediff_BionicArm_GenIII",
        "SNS_Hediff_BionicArm_GenIV",
        "SimpleProstheticArm",
        "SteelArm",
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaProsthetic(this HediffDef @this)
    {
        return UmbrellaProsthetics.Contains(@this.defName);
    }
}
