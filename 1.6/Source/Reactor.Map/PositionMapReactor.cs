using Bumbershoots.Ext.System.Collections.Generic;
using System.Linq;

namespace Bumbershoots.Reactor.Map;

internal class PositionMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    protected override void OnTick(int _)
    {
        var pos = mapState.TakeDirtyPositions();
        if (pos.Count == 0) return;
        mapState.States
            .Where(ps => pos.Contains(ps.Pawn.Position))
            .ForEach(ps => ps.Dirty());
    }
}
