#pragma warning disable IDE0044

using HarmonyLib;
using System;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Patch.Mod.Nps;

internal static class NpsSettings
{
    private class Proxy
    {
        private Traverse allowPawnsToGetWet;

        internal bool? AllowPawnsToGetWet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => allowPawnsToGetWet?.GetValue<bool>();
        }

        internal Proxy()
        {
            if (AccessTools.TypeByName("TKKN_NPS.Settings") is not Type type) return;
            var field = new Traverse(type).Field("allowPawnsToGetWet");
            if (field.IsField) allowPawnsToGetWet = field;
        }
    }

    private static readonly Lazy<Proxy> settings = new(() => new());

    internal static bool? AllowPawnsToGetWet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => settings.Value.AllowPawnsToGetWet;
    }
}
