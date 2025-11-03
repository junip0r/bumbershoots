using RimWorld;
using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace Bumbershoots;

internal partial class MapState : MapComponent
{
    private void OnThingSpawned(Thing thing)
    {
        try
        {
            if (thing is not Pawn p) return;
#if DEBUG_MAPSTATE
            LogEvent(nameof(OnThingSpawned), $"pawn spawned: {p}");
#endif
            thingsSpawned.Add(p);
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(OnThingSpawned), e));
        }
    }

    private void OnThingDespawned(Thing thing)
    {
        try
        {
            if (pawns.Count == 0) return;
            if (thing is not Pawn p) return;
#if DEBUG_MAPSTATE
            LogEvent(nameof(OnThingDespawned), $"pawn despawned: {p}");
#endif
            thingsDespawned.Add(p);
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(OnThingDespawned), e));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void OnApparelAdded(Pawn p, Apparel apparel)
    {
        try
        {
#if DEBUG_MAPSTATE
            LogEvent(nameof(OnApparelAdded), $"apparel added: {p}: {apparel}");
#endif
            apparelAdded.Add((p, apparel.def));
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(OnApparelAdded), e));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void OnApparelRemoved(Pawn p, Apparel apparel)
    {
        try
        {
            if (pawns.Count == 0) return;
#if DEBUG_MAPSTATE
            LogEvent(nameof(OnApparelRemoved), $"apparel removed: {p}: {apparel}");
#endif
            apparelRemoved.Add((p, apparel.def));
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(OnApparelAdded), e));
        }
    }

    private void OnRoofChanged(IntVec3 pos)
    {
        try
        {
            if (pawns.Count == 0) return;
#if DEBUG_MAPSTATE
            LogEvent(nameof(OnRoofChanged), $"roof changed: {pos}");
#endif
            if (pawnsAllDirty) return;
            positionsDirty.Add(pos);
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(OnRoofChanged), e));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void OnPawnStep(Pawn_PathFollower p)
    {
        try
        {
            if (pawns.Count == 0) return;
#if DEBUG_MAPSTATE
            // LogEvent(nameof(OnPawnStep), $"pawn stepped: {pawn}");
#endif
            if (pawnsAllDirty) return;
            pawnSteps.Add(p);
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(OnPawnStep), e));
        }
    }
}
