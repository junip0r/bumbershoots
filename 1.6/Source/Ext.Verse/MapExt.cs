using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class MapExt
{
    internal static MapState MapState(this Map map)
    {
        return map.GetComponent<MapState>();
    }
}
