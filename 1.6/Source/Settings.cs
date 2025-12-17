using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Settings : ModSettings
{
    public static string Category => "Mod".Translate();

    public static bool showUmbrellas = true;
    public static bool umbrellasBlockSun = true;
    public static bool umbrellaClothing = true;
    public static bool encumberWork = true;
    public static bool encumberCombat = true;

    internal static int HashCode
    {
        get
        {
            return (
                showUmbrellas,
                umbrellasBlockSun,
                umbrellaClothing,
                encumberWork,
                encumberCombat
            ).GetHashCode();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref showUmbrellas, nameof(showUmbrellas), true);
        Scribe_Values.Look(ref umbrellasBlockSun, nameof(umbrellasBlockSun), true);
        Scribe_Values.Look(ref umbrellaClothing, nameof(umbrellaClothing), true);
        Scribe_Values.Look(ref encumberWork, nameof(encumberWork), true);
        Scribe_Values.Look(ref encumberCombat, nameof(encumberCombat), true);
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
        Checkbox(l, nameof(showUmbrellas), ref showUmbrellas);
        if (ModsConfig.BiotechActive)
        {
            Checkbox(l, nameof(umbrellasBlockSun), ref umbrellasBlockSun);
        }
        Checkbox(l, nameof(umbrellaClothing), ref umbrellaClothing);
        Checkbox(l, nameof(encumberWork), ref encumberWork);
        Checkbox(l, nameof(encumberCombat), ref encumberCombat);
        l.End();
        if (Current.ProgramState == ProgramState.Playing)
        {
            GameComp.settingsHashCode = HashCode;
        }
    }
}
