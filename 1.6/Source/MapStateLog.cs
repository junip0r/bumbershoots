using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

internal partial class MapState : MapComponent
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LogErr(string method, string msg)
    {
        Log.E(map, $"! {typeof(MapState)}.{method} ! {msg}");
    }

#if DEBUG_MAPSTATE
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LogComp(string method, string msg)
    {
        Log.I(map, $": {typeof(MapState)}.{method} : {msg}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LogEvent(string method, string msg)
    {
        Log.I(map, $"* {typeof(MapState)}.{method} * {msg}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LogTick(string method, string msg)
    {
        Log.I(map, $"> {typeof(MapState)}.{method} > {msg}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void LogUtil(string method, string msg)
    {
        Log.I(map, $"- {typeof(MapState)}.{method} - {msg}");
    }
#endif
}
