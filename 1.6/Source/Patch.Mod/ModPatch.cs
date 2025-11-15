using Bumbershoots.Util;
using HarmonyLib;
using System;

namespace Bumbershoots.Patch.Mod;

internal abstract class ModPatch
{
    internal abstract LazyModMetaData Mod { get; }
    protected abstract Type[] ModTypes { get; }

    internal void PatchAll(Harmony harmony)
    {
        if (!Mod.Active) return;
        foreach (var type in ModTypes)
        {
            var modType = (ModType)type.CreateInstance();
            modType.PatchAll(harmony, Mod.Value);
        }
    }
}
