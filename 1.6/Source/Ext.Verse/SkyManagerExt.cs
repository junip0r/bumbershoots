#pragma warning disable IDE0044

using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class SkyManagerExt
{
    // see RimWorld.SanguophageUtility.InSunlight()
    internal static float SkyGlowDark = 0.1f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSunlit(this SkyManager @this) => @this.CurSkyGlow > SkyGlowDark;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaSunlight(this SkyManager @this)
    {
        return Mod.Settings.UmbrellasBlockSun && @this.IsSunlit();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsUmbrellaSunlight(this SkyManager @this, Pawn p)
    {
        return @this.IsUmbrellaSunlight() && p.HasSunlightSensitivity();
    }
}
