namespace Bumbershoots.Reactor.Map;

internal class PawnStateMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    protected override void OnTick(int _)
    {
        mapState
            .TakeDirtyPawnStates()
            .ForEach(ps => ps.Update());
    }
}
