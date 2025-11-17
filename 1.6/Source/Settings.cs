using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Settings : ModSettings
{
    public static string Category => "Mod".Translate();

    public static bool ShowUmbrellas = true;
    public static bool UmbrellasBlockSun = true;
    public static bool UmbrellaClothing = true;
    public static bool EncumberWork = true;
    public static bool EncumberCombat = true;

    internal static int HashCode
    {
        get
        {
            return (
                ShowUmbrellas,
                UmbrellasBlockSun,
                UmbrellaClothing,
                EncumberWork,
                EncumberCombat
            ).GetHashCode();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowUmbrellas, nameof(ShowUmbrellas), true);
        Scribe_Values.Look(ref UmbrellasBlockSun, nameof(UmbrellasBlockSun), true);
        Scribe_Values.Look(ref UmbrellaClothing, nameof(UmbrellaClothing), true);
        Scribe_Values.Look(ref EncumberWork, nameof(EncumberWork), true);
        Scribe_Values.Look(ref EncumberCombat, nameof(EncumberCombat), true);
    }

    internal void DoWindowContents(Rect rect)
    {
        static void Checkbox(Listing_Standard l, string name, ref bool value)
        {
            l.CheckboxLabeled(name.Translate(), ref value, $"{name}Help".Translate());
        }

        rect.width = 500f;
        var l = new Listing_Standard();
        l.Begin(rect);
        Checkbox(l, nameof(ShowUmbrellas), ref ShowUmbrellas);
        if (ModsConfig.BiotechActive)
        {
            Checkbox(l, nameof(UmbrellasBlockSun), ref UmbrellasBlockSun);
        }
        Checkbox(l, nameof(UmbrellaClothing), ref UmbrellaClothing);
        Checkbox(l, nameof(EncumberWork), ref EncumberWork);
        Checkbox(l, nameof(EncumberCombat), ref EncumberCombat);
        l.End();
        if (Current.ProgramState == ProgramState.Playing)
        {
            GameComp.SettingsHashCode = HashCode;
        }
    }
}
