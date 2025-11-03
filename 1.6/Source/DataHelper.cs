using HarmonyLib;
using Bumbershoots.Ext.Verse;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace Bumbershoots;

internal static class DataHelper
{
    private static readonly Dictionary<Pawn_PathFollower, Pawn> pawn_PathFollower_Pawn = [];
    private static readonly Dictionary<Pawn_PathFollower, MapState> pawn_PathFollower_MapState = [];
    private static readonly Dictionary<Pawn_PathFollower, Traverse> pawn_PathFollower_LastCell = [];

#if DEBUG_DATAHELPER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void I(string method, string msg)
    {
        Log.I($"[ *** ] | {typeof(DataHelper)}.{method} | {msg}");
    }
#endif

    internal static void Register(Map map)
    {
#if DEBUG_DATAHELPER
        I(nameof(Register), $"register map {map}");
#endif
        map.events.ThingSpawned += OnThingSpawned;
        map.events.ThingDespawned += OnThingDespawned;
    }

    internal static void Unregister(Map map)
    {
#if DEBUG_DATAHELPER
        I(nameof(Unregister), $"unregister map {map}");
#endif
        map.events.ThingSpawned -= OnThingSpawned;
        map.events.ThingDespawned -= OnThingDespawned;
    }

    private static void OnThingSpawned(Thing t)
    {
        if (t is not Pawn p) return;
        if (p.MapHeld is not Map m) return;
        if (p.PawnState() is not PawnState _) return;
#if DEBUG_DATAHELPER
        I(nameof(OnThingSpawned), $"register pawn {p}");
#endif
        pawn_PathFollower_Pawn[p.pather] = p;
        pawn_PathFollower_MapState[p.pather] = m.MapState();
        pawn_PathFollower_LastCell[p.pather] = new Traverse(p.pather).Field("lastCell");
    }

    private static void OnThingDespawned(Thing t)
    {
        if (t is not Pawn p) return;
        if (p.PawnState() is not PawnState _) return;
#if DEBUG_DATAHELPER
        I(nameof(OnThingDespawned), $"unregister pawn {p}");
#endif
        if (pawn_PathFollower_Pawn.Remove(p.pather))
        {
            pawn_PathFollower_MapState.Remove(p.pather);
            pawn_PathFollower_LastCell.Remove(p.pather);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Pawn Pawn_PathFollower_Pawn(Pawn_PathFollower pather)
    {
        if (pawn_PathFollower_Pawn.TryGetValue(pather, out var pawn))
        {
#if DEBUG_DATAHELPER
            I(nameof(Pawn_PathFollower_Pawn), $"=> {pawn}");
#endif
            return pawn;
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapState Pawn_PathFollower_MapState(Pawn_PathFollower pather)
    {
        if (pawn_PathFollower_MapState.TryGetValue(pather, out var mapState))
        {
#if DEBUG_DATAHELPER
            I(nameof(Pawn_PathFollower_MapState), $"=> {mapState.map}");
#endif
            return mapState;
        }
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IntVec3? Pawn_PathFollower_LastCell(Pawn_PathFollower pather)
    {
        if (pawn_PathFollower_LastCell.TryGetValue(pather, out var lastCell))
        {
            var pos = lastCell.GetValue<IntVec3>();
#if DEBUG_DATAHELPER
            I(nameof(Pawn_PathFollower_LastCell), $"=> {pos}");
#endif
            return pos;
        }
        return null;
    }
}
