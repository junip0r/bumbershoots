using Bumbershoots.Ext.Verse;

namespace Bumbershoots.Reactor.Map;

internal class SunlightMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    private bool? prev, cur = null;

    protected override void OnTick(int _)
    {
        prev = cur;
        cur = mapState.Map.skyManager.IsUmbrellaSunlight();
        if (cur != prev) mapState.Dirty();
    }

    protected override void OnReset() => prev = cur = null;
}
