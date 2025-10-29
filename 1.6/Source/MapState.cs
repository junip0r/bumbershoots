using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.System.Collections.Generic;
using Verse;

namespace Bumbershoots;

internal class MapState
{
    private static readonly Dictionary<Map, MapState> mapStates = new(8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapState For(Map m)
    {
        if (mapStates.TryGetValue(m, out var value))
            return value;
        return null;
    }

    private readonly Map map;
    private readonly Dictionary<Pawn, PawnState> pawnStates = new(64);
    private readonly HashSet<PawnState> dirtyPawnStates = new(64);
    private readonly List<IntVec3> dirtyPositions = new(8);

    internal Map Map
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => map;
    }

    internal bool AllDirty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => dirtyPawnStates.Count == pawnStates.Count;
    }

    internal IEnumerable<Pawn> Pawns
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pawnStates.Keys;
    }

    internal IEnumerable<PawnState> States
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pawnStates.Values;
    }

    internal PawnState this[Pawn p]
    {
        get
        {
            if (pawnStates.TryGetValue(p, out var value))
                return value;
            return null;
        }
        set
        {
            Exceptions.ThrowIfArgumentNull("value", value);
            if (pawnStates.ContainsKey(p)) return;
            pawnStates[p] = value;
            dirtyPawnStates.Add(value);
        }
    }

    internal MapState(Map map)
    {
        this.map = map;
        mapStates[map] = this;
        map.events.RoofChanged += RoofChanged;
        map.events.ThingSpawned += ThingSpawned;
        map.events.ThingDespawned += ThingDespawned;
    }

    private void RoofChanged(IntVec3 pos)
    {
        if (AllDirty) return;
        dirtyPositions.Add(pos);
    }

    private void ThingSpawned(Thing t)
    {
        if (t is not Pawn p) return;
        if (p.AnimalOrWildMan()) return;
        if (!p.apparel.HasUmbrellaOrHat()) return;
        pawnStates[p] = new PawnState(this, p);
    }

    private void ThingDespawned(Thing t)
    {
        if (t is not Pawn p) return;
        if (p.AnimalOrWildMan()) return;
        if (!p.apparel.HasUmbrellaOrHat()) return;
        pawnStates.Remove(p);
    }

    internal void MapRemoved()
    {
        mapStates.Remove(map);
    }

    internal void Remove(Pawn p)
    {
        if (pawnStates.TryGetValue(p, out var ps))
        {
            pawnStates.Remove(p);
            dirtyPawnStates.Remove(ps);
        }
    }

    internal List<PawnState> TakeDirtyPawnStates()
    {
        if (dirtyPawnStates.Count == 0) return [];
        List<PawnState> result = [.. dirtyPawnStates];
        dirtyPawnStates.Clear();
        return result;
    }

    internal List<IntVec3> TakeDirtyPositions()
    {
        if (dirtyPositions.Count == 0) return [];
        List<IntVec3> result = [.. dirtyPositions];
        dirtyPositions.Clear();
        return result;
    }

    internal void Dirty()
    {
        dirtyPositions.Clear();
        if (AllDirty) return;
        States.ForEach(dirtyPawnStates.Add);
    }

    internal void Dirty(PawnState ps)
    {
        if (AllDirty) return;
        dirtyPawnStates.Add(ps);
    }
}
