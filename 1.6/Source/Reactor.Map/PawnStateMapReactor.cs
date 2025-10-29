namespace Bumbershoots.Reactor.Map;

internal class PawnStateMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    protected override void DoTick(int _)
    {
        mapState
            .TakeDirtyPawnStates()
            .ForEach(ps => ps.Update());
    }
}
