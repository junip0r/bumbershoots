#pragma warning disable CS9113

using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class GameComp(Game _) : GameComponent()
{
    internal static int settingsHashCode;
    private int prevSettingsHashCode;

    private void Init()
    {
        settingsHashCode = Settings.HashCode;
        prevSettingsHashCode = settingsHashCode;
    }

    public override void StartedNewGame() => Init();

    public override void LoadedGame() => Init();

    public override void GameComponentTick()
    {
        if (settingsHashCode == prevSettingsHashCode) return;
        prevSettingsHashCode = settingsHashCode;
        var maps = Find.Maps;
        for (var i = 0; i < maps.Count; i++)
        {
            maps[i].Notify_SettingsChanged();
        }
    }
}
