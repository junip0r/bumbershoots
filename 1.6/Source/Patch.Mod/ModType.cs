using System;
using HarmonyLib;
using Verse;

namespace Bumbershoots.Patch.Mod;

internal abstract class ModType
{
    internal abstract string TypeName { get; }

    internal abstract Type[] ModMethods { get; }

    internal Type Type => AccessTools.TypeByName(TypeName);

    internal void PatchAll(Harmony harmony, ModMetaData mod)
    {
        foreach (var type in ModMethods)
        {
            var modMethod = (ModMethod)type.CreateInstance();
            modMethod.Patch(harmony, mod, this);
        }
    }
}