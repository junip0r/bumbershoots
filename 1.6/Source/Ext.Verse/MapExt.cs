using Prepatcher;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Ext.Verse;

public static class MapExt
{
    [PrepatcherField]
    [InjectComponent]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MapComp MapComp(this Map @this)
    {
        return @this.GetComponent<MapComp>();
    }

    public static void Notify_SettingsChanged(this Map @this)
    {
        @this.MapComp().Notify_SettingsChanged();
        Notify_SettingsChanged(@this.mapPawns.pawnsSpawned);
        Notify_SettingsChanged(@this.mapPawns.AllPawnsUnspawned);
    }

    public static void Notify_SettingsChanged(List<Pawn> pawns)
    {
        var count = pawns.Count;
        for (var i = 0; i < count; i++)
            pawns[i].Notify_SettingsChanged();
    }
}
