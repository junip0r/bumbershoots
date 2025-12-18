using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    internal Pawn pawn;
    internal MapComp mapComp;
    internal UmbrellaComp umbrellaComp;
    internal bool isWildMan;
    internal bool hasSunlightSensitivity;
    internal bool dead;

    public override void Initialize(CompProperties props)
    {
        if (parent is not Pawn p)
        {
            parent.comps.Remove(this);
            return;
        }
        base.Initialize(props);
        pawn = p;
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        isWildMan = pawn.IsWildMan();
        hasSunlightSensitivity = pawn.HasSunlightSensitivity();
        dead = pawn.health.Dead;
        mapComp = pawn.Map.MapComp();
        umbrellaComp?.Notify_PawnSpawned();
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        dead = pawn.health.Dead;
        umbrellaComp?.Notify_PawnDeSpawned();
        mapComp = null;
    }

    internal void Notify_GenesChanged()
    {
        hasSunlightSensitivity = pawn.HasSunlightSensitivity();
        umbrellaComp?.Notify_PawnGenesChanged();
    }

    internal void Notify_UmbrellaEquipped(UmbrellaComp umbrellaComp)
    {
        this.umbrellaComp = umbrellaComp;
    }

    internal void Notify_UmbrellaUnequipped()
    {
        umbrellaComp = null;
    }

    internal void Notify_SettingsChanged()
    {
        umbrellaComp?.Notify_SettingsChanged();
    }
}
