using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    internal MapComp mapComp;
    internal UmbrellaComp umbrellaComp;

    public MapComp MapComp => mapComp;
    public UmbrellaComp UmbrellaComp => umbrellaComp;

    public override void Initialize(CompProperties props)
    {
        if (ShouldDisable())
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
    }

    private bool ShouldDisable()
    {
        if (parent is not Pawn p) return true;
        if (p.IsWildMan()) return true;
        return false;
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        mapComp = (parent as Pawn).MapHeld.MapComp();
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        mapComp = null;
    }
}
