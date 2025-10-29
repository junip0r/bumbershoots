using Bumbershoots.Reactor;
using Bumbershoots.Reactor.Map;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class MapComponent : Verse.MapComponent
{
    private const int period = GenTicks.TicksPerRealSecond / 4;
    private readonly MapState mapState;
    private readonly List<IReactor> mapReactors;

    public MapComponent(Map map) : base(map)
    {
        static IReactor Periodic(IReactor r) => new PeriodicReactor(r, period);

        mapState = new(map);
        mapReactors = [
            Periodic(new SettingsMapReactor(mapState)),
            Periodic(new SunlightMapReactor(mapState)),
            Periodic(new WeatherMapReactor(mapState)),
            new PositionMapReactor(mapState),
            new PawnStateMapReactor(mapState),
        ];
    }

    public override void MapComponentTick() => mapReactors.Tick(GenTicks.TicksGame);

    public override void MapRemoved() => mapState.MapRemoved();
}
