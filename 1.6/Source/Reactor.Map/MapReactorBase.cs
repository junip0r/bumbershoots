using Bumbershoots.Reactor;

namespace Bumbershoots.Reactor.Map;

internal abstract class MapReactorBase(MapState mapState) : ReactorBase
{
    protected readonly MapState mapState = mapState;
}
