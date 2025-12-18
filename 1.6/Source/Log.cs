using System.Runtime.CompilerServices;

namespace Bumbershoots;

internal static class Log
{
    // Format

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string Format(string msg)
    {
        return $"[☔] {msg}";
    }

    // Info

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void I(string msg)
    {
        Verse.Log.Message(Format(msg));
    }

    // Warning

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void W(string msg)
    {
        Verse.Log.Warning(Format(msg));
    }

    // Error

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void E(string msg)
    {
        Verse.Log.Error(Format(msg));
    }
}
