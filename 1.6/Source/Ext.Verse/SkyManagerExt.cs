#pragma warning disable IDE0044

using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class SkyManagerExt
{
    // see RimWorld.SanguophageUtility.InSunlight()
    private static float skyGlowDarkness = 0.1f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSunlit(this SkyManager s) => s.CurSkyGlow > skyGlowDarkness;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaSunlight(this SkyManager s)
    {
        return Mod.Settings.UmbrellasBlockSun && IsSunlit(s);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaSunlight(this SkyManager s, Pawn p)
    {
        return IsUmbrellaSunlight(s) && p.HasSunlightSensitivity();
    }
}
