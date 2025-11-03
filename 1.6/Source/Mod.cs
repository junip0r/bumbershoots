using HarmonyLib;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Mod : Verse.Mod
{
    private const string ID = "junip0r.bumbershoots";
    private static Settings settings;

    public static Settings Settings
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => settings;
    }

    public Mod(ModContentPack pack) : base(pack)
    {
        settings = GetSettings<Settings>();
        ApplyPatches();
    }

    private void ApplyPatches()
    {
        var harmony = new Harmony(ID);
        harmony.PatchAll();
        ModPatches.PatchAll(harmony);
    }

    public override string SettingsCategory() => Settings.Category;

    public override void DoSettingsWindowContents(Rect rect)
    {
        settings.DoWindowContents(rect);
    }
}
