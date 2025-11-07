using Verse;

namespace Bumbershoots;

internal partial class MapState : MapComponent
{
    public MapState(Map map) : base(map)
    {
#if DEBUG_MAPSTATE
        LogComp(nameof(MapState), "new");
#endif
        map.events.ThingSpawned += OnThingSpawned;
        map.events.ThingDespawned += OnThingDespawned;
        map.events.RoofChanged += OnRoofChanged;
    }

    public override void MapComponentTick()
    {
        if (pawns.Count == 0)
        {
            TickIdle();
        }
        else
        {
            TickBusy();
        }
    }

    private void TickIdle()
    {
#if DEBUG_MAPSTATE
        TickDebug();
#endif
        TickThingsSpawned();
        TickApparelAdded();
        TickPawnsDirty();
        TickExceptions();
    }

    private void TickBusy()
    {
#if DEBUG_MAPSTATE
        TickDebug();
#endif
        TickThingsDespawned();
        TickApparelRemoved();
        TickSettingsChanged();
        TickSunlightChanged();
        TickWeatherChanged();
        TickPositionsDirty();
        TickPawnSteps();
        TickThingsSpawned();
        TickApparelAdded();
        TickPawnsDirty();
        TickExceptions();
    }

#if DEBUG_MAPSTATE
    public override void FinalizeInit()
    {
        LogComp(nameof(FinalizeInit), "init");
    }

    public override void MapGenerated()
    {
        LogComp(nameof(MapGenerated), "map generated");
    }
#endif

#if DEBUG_MAPSTATE
    public override void MapRemoved()
    {
        LogComp(nameof(MapRemoved), "map removed");
    }
#endif
}
