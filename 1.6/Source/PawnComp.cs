using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    // This comp exists to make MapComp easily accessible to UmbrellaComp, and
    // to make UmbrellaComp easily accessible to GameComp and the game patches.
    //
    // The UmbrellaComp for a worn umbrella will keep a reference to the pawn's
    // PawnComp, and so also the MapComp. MapComp caches some values each tick
    // so every UmbrellaComp doesn't have to compute them. It also has the map
    // as a naked field, which is faster to access than Pawn.Map or Pawn.MapHeld.
    //
    // UmbrellaComp will set itself here when worn by a pawn, and remove itself
    // when dropped. Storing it here gives it to anyone who has the pawn at the
    // cost of one hash lookup (PawnComp) and one null check (UmbrellaComp).
    //
    // On settings change, GameComp will find all pawns with a PawnComp, get the
    // UmbrellaComp (if not null), and notify it that the settings have changed.
    //
    // Absence of a PawnComp means the pawn can't use an umbrella, i.e. wild men,
    // non-humanlikes.

    internal MapComp MapComp;
    internal UmbrellaComp UmbrellaComp;

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
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        MapComp = null;
    }

    private bool ShouldDisable()
    {
        if (parent is not Pawn p) return true;
        // my xpath-fu is not yet good enough to filter wild men in the xml
        if (p.IsWildMan()) return true;
        return false;
    }
}
