using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

internal partial class MapState : MapComponent
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void StartTrackingPawn(Pawn pawn)
    {
#if DEBUG_MAPSTATE
        LogUtil(nameof(StartTrackingPawn), $"start tracking pawn: {pawn}");
#endif
        pawns.Add(pawn);
        if (!pawnsAllDirty)
        {
            pawnsDirty.Add(pawn);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void StopTrackingPawn(Pawn pawn)
    {
        if (pawns.Remove(pawn))
        {
#if DEBUG_MAPSTATE
            LogUtil(nameof(StopTrackingPawn), $"stop tracking pawn: {pawn}");
#endif
            pawn.GetComp<PawnState>().Clear();
        }
    }

    private void Dirty()
    {
#if DEBUG_MAPSTATE
        LogUtil(nameof(Dirty), "set all pawns dirty");
#endif
        pawnsAllDirty = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Dirty(Pawn pawn)
    {
#if DEBUG_MAPSTATE
        LogUtil(nameof(Dirty), $"set pawn dirty: {pawn}");
#endif
        if (pawnsAllDirty) return;
        pawnsDirty.Add(pawn);
    }
}
