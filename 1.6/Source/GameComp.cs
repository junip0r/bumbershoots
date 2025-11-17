#pragma warning disable CS9113

using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class GameComp(Game _) : GameComponent()
{
    internal static int SettingsHashCode;
    private int prevSettingsHashCode;

    public override void StartedNewGame()
    {
        SettingsHashCode = Settings.HashCode;
        prevSettingsHashCode = SettingsHashCode;
    }

    public override void LoadedGame()
    {
        SettingsHashCode = Settings.HashCode;
    }

    public override void GameComponentTick()
    {
        if (SettingsHashCode == prevSettingsHashCode) return;
        prevSettingsHashCode = SettingsHashCode;
        var maps = Find.Maps;
        for (var i = 0; i < maps.Count; i++)
        {
            maps[i].Notify_SettingsChanged();
        }
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref prevSettingsHashCode, nameof(prevSettingsHashCode));
    }
}
