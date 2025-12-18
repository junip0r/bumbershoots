using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

internal static class GeneDefExt
{
    private static readonly List<string> uvSensitivityDefNames =
    [
        "UVSensitivity_Mild",
        "UVSensitivity_Intense",
    ];

    private static GeneDef[] uvSensitivityDefs = null;

    internal static GeneDef[] UVSensitivityDefs
    {
        get
        {
            if (uvSensitivityDefs is null)
            {
                List<GeneDef> genes = new(uvSensitivityDefNames.Count);
                for (var i = 0; i < uvSensitivityDefNames.Count; i++)
                {
                    var def = DefDatabase<GeneDef>.GetNamedSilentFail(uvSensitivityDefNames[i]);
                    if (def != null) genes.Add(def);
                }
                uvSensitivityDefs = [.. genes];
            }
            return uvSensitivityDefs;
        }
    }
}
