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
        mapComp = pawn.Map.MapComp();
        umbrellaComp?.Notify_PawnSpawned();
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        umbrellaComp?.Notify_PawnDeSpawned();
        mapComp = null;
    }

    public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
    {
        umbrellaComp?.Notify_PawnKilled();
    }

    internal void Notify_Resurrected()
    {
        umbrellaComp?.Notify_PawnResurrected();
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
