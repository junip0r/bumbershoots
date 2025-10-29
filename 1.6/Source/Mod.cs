using HarmonyLib;
using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Mod : Verse.Mod
{
    private const string ID = "junip0r.bumbershoots";

    private static readonly Harmony harmony = new(ID);

    private static Settings settings;

    internal static Harmony Harmony => harmony;

    public static Settings Settings => settings;

    public Mod(ModContentPack pack) : base(pack)
    {
        ApplyPatches();
        settings = GetSettings<Settings>();
    }

    private void ApplyPatches()
    {
        Harmony.PatchAll();
        if (Mods.NaturesPrettySweet.Present)
            Patch.TKKN_NPS.Pawn_TickPatch.Patch(harmony);
    }

    public override string SettingsCategory() => Settings.Category;

    public override void DoSettingsWindowContents(Rect rect)
    {
        settings.DoWindowContents(rect);
    }
}
