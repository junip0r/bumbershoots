using System.Collections.Generic;
using Verse;

namespace Bumbershoots.Ext.RimWorld;

public static class GeneDefExt
{
    public static readonly List<string> uvSensitivityDefNames =
    [
        "UVSensitivity_Mild",
        "UVSensitivity_Intense",
    ];

    public static GeneDef[] uvSensitivityDefs = null;

    public static GeneDef[] UVSensitivityDefs
    {
        get
        {
            if (uvSensitivityDefs == null)
            {
                List<GeneDef> genes = new(uvSensitivityDefNames.Count);
                var count = uvSensitivityDefNames.Count;
                for (var i = 0; i < count; i++)
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
