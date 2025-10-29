using Bumbershoots.Ext.Verse;

namespace Bumbershoots.Reactor.Map;

internal class SunlightMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    private bool? prev, cur = null;

    protected override void DoTick(int _)
    {
        prev = cur;
        cur = mapState.Map.skyManager.IsUmbrellaSunlight();
        if (!prev.HasValue) return;
        if (cur != prev) mapState.Dirty();
    }

    protected override void DoReset() => prev = cur = null;
}
