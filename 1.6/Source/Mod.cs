using HarmonyLib;
using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Mod : Verse.Mod
{
    public const string ID = "junip0r.bumbershoots";
    public static Settings settings;

    public Mod(ModContentPack pack) : base(pack)
    {
        settings = GetSettings<Settings>();
        ApplyPatches();
    }

    public void ApplyPatches()
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
