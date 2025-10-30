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

    internal bool AllPawnStatesDirty
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (pawnStates.TryGetValue(p, out var value))
                return value;
            return null;
        }
    }

    internal MapState(Map map)
    {
        this.map = map;
        mapStates[map] = this;
        map.events.RoofChanged += OnRoofChanged;
        map.events.ThingSpawned += OnThingSpawned;
        map.events.ThingDespawned += OnThingDespawned;
    }

    private void OnRoofChanged(IntVec3 pos)
    {
        if (AllPawnStatesDirty) return;
        dirtyPositions.Add(pos);
    }

    private void OnThingSpawned(Thing t)
    {
        if (t is not Pawn p) return;
        if (p.AnimalOrWildMan()) return;
        if (!p.apparel.HasUmbrellaOrHat()) return;
        pawnStates[p] = new(this, p);
    }

    private void OnThingDespawned(Thing t)
    {
        if (t is not Pawn p) return;
        if (p.AnimalOrWildMan()) return;
        if (!p.apparel.HasUmbrellaOrHat()) return;
        pawnStates.Remove(p);
    }

    internal void OnMapRemoved()
    {
        mapStates.Remove(map);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Add(PawnState ps)
    {
        var p = ps.Pawn;
        Exceptions.ThrowIfArgumentNull("ps", ps);
        if (pawnStates.ContainsKey(p)) return;
        pawnStates[p] = ps;
        dirtyPawnStates.Add(ps);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Remove(Pawn p)
    {
        if (pawnStates.TryGetValue(p, out var ps))
        {
            pawnStates.Remove(p);
            dirtyPawnStates.Remove(ps);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal List<PawnState> TakeDirtyPawnStates()
    {
        List<PawnState> result = [.. dirtyPawnStates];
        dirtyPawnStates.Clear();
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal List<IntVec3> TakeDirtyPositions()
    {
        if (AllPawnStatesDirty)
        {
            dirtyPositions.Clear();
            return [];
        }
        List<IntVec3> result = [.. dirtyPositions];
        dirtyPositions.Clear();
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Dirty()
    {
        if (AllPawnStatesDirty) return;
        States.ForEach(dirtyPawnStates.Add);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Dirty(PawnState ps)
    {
        if (AllPawnStatesDirty) return;
        dirtyPawnStates.Add(ps);
    }
}
