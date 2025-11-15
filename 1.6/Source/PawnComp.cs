using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    internal MapComp MapComp;
    internal UmbrellaComp UmbrellaComp;

    internal Pawn Pawn => parent as Pawn;

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
        MapComp = Pawn.MapHeld.MapComp();
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        MapComp = null;
    }

    private bool ShouldDisable()
    {
        if (parent is not Pawn p) return true;
        if (p.IsWildMan()) return true;
        return false;
    }
}
