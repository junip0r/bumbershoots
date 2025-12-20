using Bumbershoots.Ext.Verse;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

public class UmbrellaComp : ThingComp
{
    public static readonly List<PawnRenderNodeTagDef> renderNodeTags =
    [
        PawnRenderNodeTagDefOf.ApparelHead,
        PawnRenderNodeTagDefOf.ApparelBody,
    ];

    public Apparel apparel;
    public UmbrellaProps umbrellaProps;
    public UmbrellaHediffs umbrellaHediffs;
    public PawnComp pawnComp;
    public bool canBlockSunlight;
    public bool canBlockWeather;
    public bool activated;
    public bool ticking;

    public bool BlockingSunlight => activated && canBlockSunlight;
    public bool BlockingWeather => activated && canBlockWeather;

    public bool ShouldDisable =>
        apparel == null || !umbrellaProps.IsForDef(apparel.def.defName);

    public override void Initialize(CompProperties props)
    {
        umbrellaProps = (UmbrellaProps)props;
        apparel = parent as Apparel;
        if (ShouldDisable)
        {
            parent.comps.Remove(this);
            return;
        }
        base.Initialize(props);
        umbrellaHediffs = new(umbrellaProps);
    }

    public override void PostExposeData()
    {
        Scribe_Values.Look(ref canBlockSunlight, nameof(canBlockSunlight));
        Scribe_Values.Look(ref canBlockWeather, nameof(canBlockWeather));
        Scribe_Values.Look(ref activated, nameof(activated));

        if (Scribe.mode == LoadSaveMode.PostLoadInit)
            Notify_Equipped(apparel.Wearer);
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

    public void Notify_StateChanged()
    {
        ticking = canBlockWeather || canBlockSunlight;
        if (!ticking && activated && !pawnComp.dead) Deactivate();
    }

    public override void Notify_Equipped(Pawn pawn)
    {
        pawnComp = pawn?.PawnComp();
        if (pawnComp == null) return;
        pawnComp.Notify_UmbrellaEquipped(this);
        if (Scribe.mode == LoadSaveMode.PostLoadInit)
            umbrellaHediffs.Load(pawn);
        else
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
        ConnectMapComp();
        Notify_SunlightChanged();
        Notify_WeatherChanged();
        CompTick();
    }

    public void Notify_PawnDeSpawned()
    {
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
            && pawnComp.mapComp.curIsSunlight == true
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
    }

    public void ConnectMapComp()
    {
        DisconnectMapComp();
        pawnComp.mapComp.SunlightChanged += Notify_SunlightChanged;
        pawnComp.mapComp.WeatherChanged += Notify_WeatherChanged;
    }

    public void DisconnectMapComp()
    {
        pawnComp.mapComp.SunlightChanged -= Notify_SunlightChanged;
        pawnComp.mapComp.WeatherChanged -= Notify_WeatherChanged;
    }

    public void Activate()
    {
        activated = true;
        umbrellaHediffs.Activate();
        DirtyGraphics();
    }

    public void Deactivate()
    {
        activated = false;
        umbrellaHediffs.Deactivate();
        DirtyGraphics();
    }

    public void DirtyGraphics()
    {
        if (!pawnComp.pawn.Spawned) return;
        DirtyUmbrellaGraphics();
        DirtyPortraitGraphics();
    }

    public void DirtyUmbrellaGraphics()
    {
        var renderTree = pawnComp.pawn.Drawer.renderer.renderTree;
        if (!renderTree.Resolved) return;
        var count = renderNodeTags.Count;
        for (var i = 0; i < count; i++)
        {
            var node = renderTree.nodesByTag.GetValueOrDefault(renderNodeTags[i]);
            if (node?.children == null) continue;
            var length = node.children.Length;
            for (var j = 0; j < length; j++)
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

    public void DirtyPortraitGraphics()
    {
        PortraitsCache.SetDirty(pawnComp.pawn);
    }
}
