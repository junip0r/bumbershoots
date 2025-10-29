namespace Bumbershoots.Reactor.Map;

internal class SettingsMapReactor(MapState mapState) : MapReactorBase(mapState)
{
    private Settings prev, cur = null;

    protected override void DoTick(int _)
    {
        prev = cur;
        cur = Mod.Settings.Copy();
        if (prev is null) return;
        if (!cur.Equals(prev)) mapState.Dirty();
    }

    protected override void DoReset() => prev = cur = null;
}
