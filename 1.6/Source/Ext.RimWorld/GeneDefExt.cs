using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class GeneDefExt
{
    private static readonly List<string> uvSensitivityGeneDefNames =
    [
        "UVSensitivity_Mild",
        "UVSensitivity_Intense",
    ];

    private static GeneDef[] uvSensitivityGeneDefs = null;

    internal static GeneDef[] UVSensitivityGeneDefs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (uvSensitivityGeneDefs is null)
            {
                List<GeneDef> genes = new(uvSensitivityGeneDefNames.Count);
                for (var i = 0; i < uvSensitivityGeneDefNames.Count; i++)
                {
                    var def = DefDatabase<GeneDef>.GetNamedSilentFail(uvSensitivityGeneDefNames[i]);
                    if (def != null) genes.Add(def);
                }
                uvSensitivityGeneDefs = [.. genes];
            }
            return uvSensitivityGeneDefs;
        }
    }
}
