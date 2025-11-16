#pragma warning disable CS9113

using System.Runtime.CompilerServices;
using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class GameComp(Game _) : GameComponent()
{
    internal static int SettingsHashCode;

    private int prevSettingsHashCode;

    public override void StartedNewGame()
    {
        SettingsHashCode = Mod.Settings.GetHashCode();
        prevSettingsHashCode = SettingsHashCode;
    }

    public override void LoadedGame()
    {
        SettingsHashCode = Mod.Settings.GetHashCode();
    }

    public override void GameComponentTick()
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Notify(Pawn pawn)
        {
            if (pawn.AnimalOrWildMan()) return;
            if (pawn.UmbrellaComp() is not UmbrellaComp umbrellaComp) return;
            umbrellaComp.Notify_SettingsChanged();
        }

        if (SettingsHashCode == prevSettingsHashCode) return;
        prevSettingsHashCode = SettingsHashCode;
        var maps = Find.Maps;
        for (var i = 0; i < maps.Count; i++)
        {
            var pawns = maps[i].mapPawns.AllPawnsSpawned;
            for (var j = 0; j < pawns.Count; j++)
            {
                Notify(pawns[j]);
            }
            pawns = maps[i].mapPawns.AllPawnsUnspawned;
            for (var j = 0; j < pawns.Count; j++)
            {
                Notify(pawns[j]);
            }
        }
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref prevSettingsHashCode, nameof(prevSettingsHashCode));
    }
}
