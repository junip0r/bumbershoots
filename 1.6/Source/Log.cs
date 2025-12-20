using System.Runtime.CompilerServices;

namespace Bumbershoots;

public static class Log
{
    // Format

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(string msg)
    {
        return $"[☔] {msg}";
    }

    // Info

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void I(string msg)
    {
        Verse.Log.Message(Format(msg));
    }

    // Warning

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void W(string msg)
    {
        Verse.Log.Warning(Format(msg));
    }

    // Error

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void E(string msg)
    {
        Verse.Log.Error(Format(msg));
    }
}
