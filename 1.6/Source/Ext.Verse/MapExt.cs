using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class MapExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapState MapState(this Map map)
    {
        return map.GetComponent<MapState>();
    }
}
