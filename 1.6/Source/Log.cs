using System.Runtime.CompilerServices;

namespace Bumbershoots;

internal static class Log
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void E(string msg)
    {
        Verse.Log.Error($"[☔] {msg}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void M(string msg)
    {
        Verse.Log.Message($"[☔] {msg}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void W(string msg)
    {
        Verse.Log.Warning($"[☔] {msg}");
    }
}
