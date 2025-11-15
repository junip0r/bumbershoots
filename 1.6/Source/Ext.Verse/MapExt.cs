using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class MapExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapComp MapComp(this Map @this)
    {
        return @this.GetComponent<MapComp>();
    }
}
