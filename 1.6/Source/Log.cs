using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

internal static class Log
{
    // Format

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string Format(Map map)
    {
        return map is null ? "null" : map.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string Format(string msg)
    {
        return $"[☔] {msg}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string Format(Map map, string msg)
    {
        return $"[☔][{Format(map)}] {msg}";
    }

    // Info

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void I(string msg)
    {
        Verse.Log.Message(Format(msg));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void I(Map map, string msg)
    {
        Verse.Log.Message(Format(map, msg));
    }

    // Warning

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void W(string msg)
    {
        Verse.Log.Warning(Format(msg));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void W(Map map, string msg)
    {
        Verse.Log.Warning(Format(map, msg));
    }

    // Error

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void E(string msg)
    {
        Verse.Log.Error(Format(msg));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void E(Map map, string msg)
    {
        Verse.Log.Error(Format(map, msg));
    }
}
