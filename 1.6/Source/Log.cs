namespace Bumbershoots;

internal static class Log
{
    internal static void E(string msg)
    {
        Verse.Log.Error($"[☔] {msg}");
    }

    internal static void M(string msg)
    {
        Verse.Log.Message($"[☔] {msg}");
    }

    internal static void W(string msg)
    {
        Verse.Log.Warning($"[☔] {msg}");
    }
}
