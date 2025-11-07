using HarmonyLib;
using System.Reflection;
using System.Runtime.CompilerServices;
using Verse.AI;
using Verse;

namespace Bumbershoots.Ext.Verse.AI;

internal static class Pawn_PathFollowerExt
{
    private static readonly FieldInfo pawn = AccessTools.Field(typeof(Pawn_PathFollower), "pawn");
    private static readonly FieldInfo lastCell = AccessTools.Field(typeof(Pawn_PathFollower), "lastCell");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Pawn Pawn(this Pawn_PathFollower @this) => (Pawn)pawn.GetValue(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IntVec3 LastCell(this Pawn_PathFollower @this) => (IntVec3)lastCell.GetValue(@this);
}
