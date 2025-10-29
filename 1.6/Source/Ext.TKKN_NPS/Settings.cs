#pragma warning disable IDE0044

using HarmonyLib;
using System;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Ext.TKKN_NPS;

internal static class Settings
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
            var f = new Traverse(type).Field("allowPawnsToGetWet");
            if (f.IsField) allowPawnsToGetWet = f;
        }
    }

    private static readonly Lazy<Proxy> settings = new(() => new());

    internal static bool? AllowPawnsToGetWet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => settings.Value.AllowPawnsToGetWet;
    }
}
