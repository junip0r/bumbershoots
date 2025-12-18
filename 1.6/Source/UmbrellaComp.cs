using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    private static readonly List<PawnRenderNodeTagDef> renderNodeTags =
    [
        PawnRenderNodeTagDefOf.ApparelHead,
        PawnRenderNodeTagDefOf.ApparelBody,
    ];

    public Apparel apparel;
    public PawnComp pawnComp;
    public UmbrellaProps umbrellaProps;
    public UmbrellaHediffs umbrellaHediffs;
    public bool canBlockSunlight;
    public bool canBlockWeather;
    public bool activated;
    public bool ticking;

    public bool BlockingSunlight => activated && canBlockSunlight;
    public bool BlockingWeather => activated && canBlockWeather;

    public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        if (parent is not Apparel a || !umbrellaProps.IsForDef(a.def.defName))
        {
            parent.comps.Remove(this);
            return;
        }
        base.Initialize(props);
        apparel = a;
        umbrellaHediffs = new(umbrellaProps);
    }

    public override void PostExposeData()
    {
        Scribe_Values.Look(ref canBlockSunlight, nameof(canBlockSunlight));
        Scribe_Values.Look(ref canBlockWeather, nameof(canBlockWeather));
        Scribe_Values.Look(ref activated, nameof(activated));

        if (Scribe.mode == LoadSaveMode.PostLoadInit) Notify_LoadSave();
    }

    public override void CompTick()
    {
        if (!ticking) return;
        if (pawnComp.pawn.Position.Roofed(pawnComp.mapComp.map))
        {
            if (activated) Deactivate();
        }
        else
        {
            if (!activated) Activate();
        }
    }

    private void Notify_StateChanged()
    {
        ticking = (canBlockWeather || canBlockSunlight) && !pawnComp.isWildMan;
        if (!ticking && activated && !pawnComp.dead) Deactivate();
    }

    private void Notify_LoadSave()
    {
        pawnComp = apparel.Wearer?.PawnComp();
        if (pawnComp == null) return;
        pawnComp.Notify_UmbrellaEquipped(this);
        umbrellaHediffs.Load(pawnComp.pawn);
        if (pawnComp.mapComp != null) Notify_PawnSpawned();
    }

    public override void Notify_Equipped(Pawn pawn)
    {
        pawnComp = apparel.Wearer?.PawnComp();
        if (pawnComp == null) return;
        pawnComp.Notify_UmbrellaEquipped(this);
        umbrellaHediffs.Add(pawn);
        if (pawnComp.mapComp != null) Notify_PawnSpawned();
    }

    public override void Notify_Unequipped(Pawn pawn)
    {
        if (pawnComp.mapComp != null) Notify_PawnDeSpawned();
        pawnComp.Notify_UmbrellaUnequipped();
        pawnComp = null;
    }

    public void Notify_PawnSpawned()
    {
        Log.W($"UmbrellaComp.Notify_PawnSpawned({pawnComp.pawn})");
        ConnectMapComp();
        Notify_SunlightChanged();
        Notify_WeatherChanged();
        CompTick();
    }

    public void Notify_PawnDeSpawned()
    {
        Log.W($"UmbrellaComp.Notify_PawnDeSpawned({pawnComp.pawn}) dead={pawnComp.dead}");
        DisconnectMapComp();
        canBlockSunlight = false;
        canBlockWeather = false;
        Notify_StateChanged();
    }

    public void Notify_PawnGenesChanged()
    {
        if (pawnComp.mapComp == null) return;
        Notify_SunlightChanged();
    }

    public void Notify_SunlightChanged()
    {
        canBlockSunlight = umbrellaProps.blocksSunlight
            && pawnComp.mapComp.isSunlight == true
            && pawnComp.hasSunlightSensitivity;
        Notify_StateChanged();
    }

    public void Notify_WeatherChanged()
    {
        var curWeatherLerped = pawnComp.mapComp.curWeatherLerped;
        canBlockWeather = umbrellaProps.BlocksWeather(curWeatherLerped);
        Notify_StateChanged();
    }

    public void Notify_SettingsChanged()
    {
        var pawn = pawnComp.pawn;
        Notify_Unequipped(pawn);
        Notify_Equipped(pawn);
        CompTick();
    }

    private void ConnectMapComp()
    {
        DisconnectMapComp();
        pawnComp.mapComp.SunlightChanged += Notify_SunlightChanged;
        pawnComp.mapComp.WeatherChanged += Notify_WeatherChanged;
    }

    private void DisconnectMapComp()
    {
        pawnComp.mapComp.SunlightChanged -= Notify_SunlightChanged;
        pawnComp.mapComp.WeatherChanged -= Notify_WeatherChanged;
    }

    private void Activate()
    {
        activated = true;
        umbrellaHediffs.Activate();
        DirtyGraphics();
    }

    private void Deactivate()
    {
        activated = false;
        umbrellaHediffs.Deactivate();
        DirtyGraphics();
    }

    private void DirtyGraphics()
    {
        if (!pawnComp.pawn.Spawned) return;
        DirtyApparelGraphics();
        DirtyPortraitGraphics();
    }

    private void DirtyApparelGraphics()
    {
        var renderTree = pawnComp.pawn.Drawer.renderer.renderTree;
        if (!renderTree.Resolved) return;
        for (var i = 0; i < renderNodeTags.Count; i++)
        {
            var node = renderTree.nodesByTag.GetValueOrDefault(renderNodeTags[i]);
            if (node?.children == null) continue;
            for (var j = 0; j < node.children.Length; j++)
            {
                var child = node.children[j];
                if (child.apparel == apparel)
                {
                    child.requestRecache = true;
                    return;
                }
            }
        }
    }

    private void DirtyPortraitGraphics()
    {
        PortraitsCache.SetDirty(pawnComp.pawn);
    }
}
