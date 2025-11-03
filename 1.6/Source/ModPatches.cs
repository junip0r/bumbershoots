using Bumbershoots.Patch.Mod;
using System;
using HarmonyLib;

namespace Bumbershoots;

internal static class ModPatches
{
    private static readonly Type[] modPatchTypes =
    [
        typeof(Patch.Mod.Nps.NpsPatch),
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
