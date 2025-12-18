using Bumbershoots.Ext.Verse;
using Verse;

namespace Bumbershoots;

public class PawnComp : ThingComp
{
    public Pawn pawn;
    public MapComp mapComp;
    public UmbrellaComp umbrellaComp;
    public bool hasSunlightSensitivity;
    public bool dead;

    public bool ShouldDisable => pawn is null || pawn.IsWildMan();

    public override void Initialize(CompProperties props)
    {
        pawn = parent as Pawn;
        if (ShouldDisable)
        {
            parent.comps.Remove(this);
            return;
        }
        base.Initialize(props);
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
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

    public void Notify_GenesChanged()
    {
        hasSunlightSensitivity = pawn.HasSunlightSensitivity();
        umbrellaComp?.Notify_PawnGenesChanged();
    }

    public void Notify_UmbrellaEquipped(UmbrellaComp umbrellaComp)
    {
        this.umbrellaComp = umbrellaComp;
    }

    public void Notify_UmbrellaUnequipped()
    {
        umbrellaComp = null;
    }

    public void Notify_SettingsChanged()
    {
        umbrellaComp?.Notify_SettingsChanged();
    }
}
