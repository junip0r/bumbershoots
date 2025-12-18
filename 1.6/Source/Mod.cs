using HarmonyLib;
using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Mod : Verse.Mod
{
    private const string ID = "junip0r.bumbershoots";
    internal static Settings settings;

    public Mod(ModContentPack pack) : base(pack)
    {
        settings = GetSettings<Settings>();
        ApplyPatches();
    }

    private void ApplyPatches()
    {
        var harmony = new Harmony(ID);
        harmony.PatchAll();
    }

    public override string SettingsCategory()
    {
        return Settings.Category;
    }

    public override void DoSettingsWindowContents(Rect rect)
    {
        settings.DoWindowContents(rect);
    }
}
