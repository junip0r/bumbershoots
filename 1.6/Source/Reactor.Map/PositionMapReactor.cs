using Bumbershoots.Ext.System.Collections.Generic;
using System.Linq;

namespace Bumbershoots.Reactor.Map;

internal class PositionMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    protected override void DoTick(int _)
    {
        var pos = mapState.TakeDirtyPositions();
        if (pos.Count == 0 || mapState.AllDirty) return;
        mapState.States
            .Where(ps => pos.Contains(ps.Pawn.Position))
            .ForEach(mapState.Dirty);
    }
}
