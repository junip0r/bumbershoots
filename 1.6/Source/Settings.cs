using System.Runtime.CompilerServices;
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
        Scribe_Values.Look(ref ShowUmbrellas, "ShowUmbrellas", true);
        Scribe_Values.Look(ref UmbrellasBlockSun, "UmbrellasBlockSun", true);
        Scribe_Values.Look(ref UmbrellaHats, "UmbrellaHats", true);
        Scribe_Values.Look(ref EncumberWork, "EncumberWork", true);
        Scribe_Values.Look(ref EncumberCombat, "EncumberCombat", true);
    }

    internal void DoWindowContents(Rect rect)
    {
        rect.width = 500f;
        var l = new Listing_Standard();
        l.Begin(rect);
        l.CheckboxLabeled("ShowUmbrellas".Translate(), ref ShowUmbrellas, "ShowUmbrellasHelp".Translate());
        if (ModsConfig.BiotechActive)
        {
            l.CheckboxLabeled("UmbrellasBlockSun".Translate(), ref UmbrellasBlockSun, "UmbrellasBlockSunHelp".Translate());
        }
        l.CheckboxLabeled("UmbrellaHats".Translate(), ref UmbrellaHats, "UmbrellaHatsHelp".Translate());
        l.CheckboxLabeled("EncumberWork".Translate(), ref EncumberWork, "EncumberWorkHelp".Translate());
        l.CheckboxLabeled("EncumberCombat".Translate(), ref EncumberCombat, "EncumberCombatHelp".Translate());
        l.End();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Settings Copy()
    {
        return new Settings()
        {
            ShowUmbrellas = ShowUmbrellas,
            UmbrellasBlockSun = UmbrellasBlockSun,
            UmbrellaHats = UmbrellaHats,
            EncumberWork = EncumberWork,
            EncumberCombat = EncumberCombat,
        };
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
