using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class HediffDefExt
{
    public static readonly HashSet<string> UmbrellaProsthetics =
    [
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

    public static readonly List<string> UmbrellaEncumbrances =
    [
        "Bumber_UmbrellaEncumbranceCombat",
        "Bumber_UmbrellaEncumbranceWork",
    ];

    public static readonly List<Func<bool>> UmbrellaEncumbranceEnabled =
    [
        static () => Mod.Settings.EncumberCombat,
        static () => Mod.Settings.EncumberWork,
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaProsthetic(this HediffDef @this)
    {
        return UmbrellaProsthetics.Contains(@this.defName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUmbrellaEncumbrance(this HediffDef @this)
    {
        for (var i = 0; i < UmbrellaEncumbrances.Count; i++)
        {
            if (@this.defName == UmbrellaEncumbrances[i]) return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddUmbrellaEncumbrance(string defName, Func<bool> enabledPredicate)
    {
        UmbrellaEncumbrances.Add(defName);
        UmbrellaEncumbranceEnabled.Add(enabledPredicate);
    }
}
