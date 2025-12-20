#pragma warning disable CS9113

using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class GameComp(Game _) : GameComponent()
{
    public static int settingsHashCode;
    public static int prevSettingsHashCode;

    public override void StartedNewGame()
    {
        settingsHashCode = Settings.HashCode;
        prevSettingsHashCode = settingsHashCode;
    }

    public override void LoadedGame()
    {
        settingsHashCode = Settings.HashCode;
    }

    public override void GameComponentTick()
    {
        if (settingsHashCode == prevSettingsHashCode) return;
        prevSettingsHashCode = settingsHashCode;
        var maps = Find.Maps;
        var count = maps.Count;
        for (var i = 0; i < count; i++)
            maps[i].Notify_SettingsChanged();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref prevSettingsHashCode, nameof(prevSettingsHashCode));
    }
}
