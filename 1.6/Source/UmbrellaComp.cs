using Bumbershoots.Ext.Verse;
using RimWorld;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    internal UmbrellaProps umbrellaProps;

    internal Pawn pawn;
    internal PawnComp pawnComp;
    internal bool activated;
    private UmbrellaHediffs hediffs;

    private bool ShouldDisable => !umbrellaProps.IsForDef(parent.def.defName);

    public override void PostExposeData()
    {
        Scribe_References.Look(ref pawn, nameof(pawn));
        Scribe_Values.Look(ref activated, nameof(activated));
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            if (pawn != null && pawn.PawnComp() is PawnComp pawnComp)
            {
                this.pawnComp = pawnComp;
                this.pawnComp.Notify_UmbrellaEquipped(this);
            }
        }
    }

    public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        if (ShouldDisable)
        {
            parent.AllComps.Remove(this);
            return;
        }
        base.Initialize(props);
        hediffs = new(umbrellaProps);
    }

    public override void Notify_Equipped(Pawn pawn)
    {
        if (pawn.PawnComp() is not PawnComp pawnComp) return;
        this.pawn = pawn;
        this.pawnComp = pawnComp;
        this.pawnComp.Notify_UmbrellaEquipped(this);
    }

    public override void Notify_Unequipped(Pawn pawn)
    {
        if (this.pawn is null) return;
        if (activated) Deactivate();
        pawnComp.Notify_UmbrellaUnequipped(this);
        pawnComp = null;
        this.pawn = null;
    }

    internal void Update()
    {
        if (pawnComp.roofed || !pawnComp.ticking)
        {
            if (activated) Deactivate();
        }
        else
        {
            if (!activated) Activate();
        }
    }

    private void Activate()
    {
        activated = true;
        hediffs.Activate(pawn);
        UpdateGraphics();
    }

    private void Deactivate()
    {
        activated = false;
        hediffs.Deactivate(pawn);
        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        if (!umbrellaProps.hideable) return;
        pawn.apparel.Notify_ApparelChanged();
        PortraitsCache.SetDirty(pawn);
    }
}
