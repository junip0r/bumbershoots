using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class GeneDefExt
{
    internal static readonly Lazy<GeneDef[]> uvSensitivityGeneDefs = new(delegate
    {
        var mild = DefDatabase<GeneDef>.GetNamedSilentFail("UVSensitivity_Mild");
        var intense = DefDatabase<GeneDef>.GetNamedSilentFail("UVSensitivity_Intense");
        List<GeneDef> genes = [];
        if (mild is not null) genes.Add(mild);
        if (intense is not null) genes.Add(intense);
        return [.. genes];
    });

    internal static GeneDef[] UVSensitivityGeneDefs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => uvSensitivityGeneDefs.Value;
    }
}
