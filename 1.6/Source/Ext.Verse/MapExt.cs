using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class MapExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapState MapState(this Map @this)
    {
        return @this.GetComponent<MapState>();
    }
}
