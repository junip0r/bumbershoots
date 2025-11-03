using UnityEngine;
using Verse;

namespace Bumbershoots;

public class Settings : ModSettings
{
    public static string Category => "ModName".Translate();

    public bool ShowUmbrellas = true;
    public bool UmbrellasBlockSun = true;
    public bool UmbrellaHats = true;
    public bool EncumberWork = true;
    public bool EncumberCombat = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ShowUmbrellas, nameof(ShowUmbrellas), true);
        Scribe_Values.Look(ref UmbrellasBlockSun, nameof(UmbrellasBlockSun), true);
        Scribe_Values.Look(ref UmbrellaHats, nameof(UmbrellaHats), true);
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
        Checkbox(l, nameof(UmbrellaHats), ref UmbrellaHats);
        Checkbox(l, nameof(EncumberWork), ref EncumberWork);
        Checkbox(l, nameof(EncumberCombat), ref EncumberCombat);
        l.End();
    }

    public override int GetHashCode()
    {
        return (
            ShowUmbrellas,
            UmbrellasBlockSun,
            UmbrellaHats,
            EncumberWork,
            EncumberCombat
        ).GetHashCode();
    }

    public override bool Equals(object o)
    {
        return o is Settings && o.GetHashCode() == GetHashCode();
    }
}
