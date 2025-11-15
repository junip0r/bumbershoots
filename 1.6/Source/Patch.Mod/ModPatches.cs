using System;
using HarmonyLib;

namespace Bumbershoots.Patch.Mod;

internal static class ModPatches
{
    private static readonly Type[] modPatchTypes =
    [
        typeof(Nps.NpsPatch),
    ];

    internal static void PatchAll(Harmony harmony)
    {
        foreach (var type in modPatchTypes)
        {
            var modPatch = (ModPatch)type.CreateInstance();
            modPatch.PatchAll(harmony);
        }
    }
}
