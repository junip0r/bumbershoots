using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

internal static class MapExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static MapComp MapComp(this Map @this)
    {
        return @this.GetComponent<MapComp>();
    }

    internal static void Notify_SettingsChanged(this Map @this)
    {
        @this.MapComp().Notify_SettingsChanged();
        Notify_SettingsChanged(@this.mapPawns.AllPawnsSpawned);
        Notify_SettingsChanged(@this.mapPawns.AllPawnsUnspawned);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Notify_SettingsChanged(IReadOnlyList<Pawn> pawns)
    {
        for (var i = 0; i < pawns.Count; i++)
        {
            pawns[i].Notify_SettingsChanged();
        }
    }
}
