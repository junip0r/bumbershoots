using System.Runtime.CompilerServices;
using Verse.AI;
using Verse;

namespace Bumbershoots.Ext.Verse.AI;

internal static class Pawn_PathFollowerExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Pawn Pawn(this Pawn_PathFollower pather)
    {
        return DataHelper.Pawn_PathFollower_Pawn(pather);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapState MapState(this Pawn_PathFollower pather)
    {
        return DataHelper.Pawn_PathFollower_MapState(pather);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IntVec3 LastCell(this Pawn_PathFollower pather)
    {
        return DataHelper.Pawn_PathFollower_LastCell(pather).Value;
    }
}
