using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    internal MapComp MapComp;
    internal UmbrellaComp UmbrellaComp;

    private bool ShouldDisable()
    {
        if (parent is not Pawn p) return true;
        if (p.IsWildMan()) return true;
        return false;
    }

    public override void Initialize(CompProperties props)
    {
        if (ShouldDisable())
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        MapComp = (parent as Pawn).MapHeld.MapComp();
        UmbrellaComp?.Notify_MapLoad();
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        UmbrellaComp?.Notify_MapUnload();
        MapComp = null;
    }
}
