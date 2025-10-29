using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
using Verse.AI;
using Verse;

namespace Bumbershoots.Ext.Verse.AI;

internal static class Pawn_PathFollowerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IntVec3 LastCell(this Pawn_PathFollower pather)
    {
        return new Traverse(pather)
            .Field("lastCell")
            .GetValue<IntVec3>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Lazy<IntVec3> LastCellLazy(this Pawn_PathFollower pather)
    {
        return new(() => LastCell(pather));
    }
}
