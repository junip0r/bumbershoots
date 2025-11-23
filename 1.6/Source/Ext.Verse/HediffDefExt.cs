using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class HediffDefExt
{
    private static readonly HashSet<string> UmbrellaProsthetics =
    [
        // FIXME figure out which mods provide these hediffs

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
    internal static bool IsUmbrellaProsthetic(this HediffDef @this)
    {
        return UmbrellaProsthetics.Contains(@this.defName);
    }
}
