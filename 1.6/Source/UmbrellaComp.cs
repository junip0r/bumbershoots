using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    private static readonly List<PawnRenderNodeTagDef> searchRenderNodeTags =
    [
        PawnRenderNodeTagDefOf.ApparelHead,
        PawnRenderNodeTagDefOf.ApparelBody,
    ];

    internal Apparel apparel;
    internal PawnComp pawnComp;
    internal UmbrellaProps umbrellaProps;
    internal UmbrellaHediffs umbrellaHediffs;
    internal bool canBlockSunlight;
    internal bool canBlockWeather;
    internal bool blockingSunlight;
    internal bool blockingWeather;
    internal bool activated;
    internal bool ticking;

    public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        if (!umbrellaProps.IsForDef(parent.def.defName))
        {
            parent.comps.Remove(this);
            return;
        }
        base.Initialize(props);
        apparel = (Apparel)parent;
        umbrellaHediffs = new(umbrellaProps);
    }

    public override void CompTick()
    {
        if (!ticking) return;
        var activate = (canBlockSunlight || canBlockWeather)
            && !pawnComp.pawn.Position.Roofed(pawnComp.mapComp.map);
        if (activate)
        {
            if (!activated) Activate();
        }
        else
        {
            if (activated) Deactivate();
        }
    }

    public override void PostExposeData()
    {
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
        {
            var wearer = apparel.Wearer;
            if (wearer != null && pawnComp == null)
            {
                Notify_Equipped(wearer);
            }
        }
    }

    private void Notify_StateChanged()
    {
        ticking = pawnComp != null
            && !pawnComp.isWildMan
            && pawnComp.mapComp != null
            && pawnComp.pawn.Spawned
            && !pawnComp.pawn.health.Dead
            && (canBlockWeather || canBlockSunlight);
        if (!ticking && activated && !pawnComp.pawn.health.Dead) Deactivate();
    }

    public override void Notify_Equipped(Pawn pawn)
    {
        if (pawn.PawnComp() is not PawnComp pawnComp) return;
        this.pawnComp = pawnComp;
        pawnComp.Notify_UmbrellaEquipped(this);
        if (pawnComp.mapComp != null)
        {
            Notify_PawnSpawned();
        }
    }

    public override void Notify_Unequipped(Pawn pawn)
    {
        if (pawnComp.mapComp != null)
        {
            Notify_PawnDeSpawned();
        }
        pawnComp.Notify_UmbrellaUnequipped();
        pawnComp = null;
    }

    internal void Notify_PawnSpawned()
    {
        pawnComp.mapComp.SunlightChanged += Notify_SunlightChanged;
        pawnComp.mapComp.WeatherChanged += Notify_WeatherChanged;
        Notify_SunlightChanged();
        Notify_WeatherChanged();
    }

    internal void Notify_PawnDeSpawned()
    {
        pawnComp.mapComp.SunlightChanged -= Notify_SunlightChanged;
        pawnComp.mapComp.WeatherChanged -= Notify_WeatherChanged;
        Notify_StateChanged();
    }

    internal void Notify_PawnKilled()
    {
        Notify_PawnDeSpawned();
    }

    internal void Notify_PawnResurrected()
    {
        Notify_PawnSpawned();
    }

    internal void Notify_PawnGenesChanged()
    {
        Notify_SunlightChanged();
    }

    internal void Notify_SunlightChanged()
    {
        if (!pawnComp.mapComp.Ready) return;
        canBlockSunlight = umbrellaProps.blocksSunlight
            && pawnComp.mapComp.isSunlight == true
            && pawnComp.hasSunlightSensitivity;
        Notify_StateChanged();
    }

    internal void Notify_WeatherChanged()
    {
        if (!pawnComp.mapComp.Ready) return;
        var curWeatherLerped = pawnComp.mapComp.curWeatherLerped;
        canBlockWeather = umbrellaProps.BlocksWeather(curWeatherLerped);
        Notify_StateChanged();
    }

    internal void Notify_SettingsChanged()
    {
        if (activated) Deactivate();
        Notify_SunlightChanged();
        Notify_WeatherChanged();
        CompTick();
    }

    private void Activate()
    {
        activated = true;
        blockingSunlight = canBlockSunlight;
        blockingWeather = canBlockWeather;
        umbrellaHediffs.Activate(pawnComp.pawn);
        DirtyGraphics();
    }

    private void Deactivate()
    {
        activated = false;
        blockingSunlight = false;
        blockingWeather = false;
        umbrellaHediffs.Deactivate(pawnComp.pawn);
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
        for (var i = 0; i < searchRenderNodeTags.Count; i++)
        {
            var node = renderTree.nodesByTag.GetValueOrDefault(searchRenderNodeTags[i]);
            if (node == null) continue;
            for (var j = 0; j < node.children.Length; j++)
            {
                var child = node.children[j];
                if (child.apparel == parent)
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
