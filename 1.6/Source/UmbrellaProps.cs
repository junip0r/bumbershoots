using System;
using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

// For usage examples, see:
//
// - Defs/Apparel_Umbrellas.xml
// - Patches/Apparel_Headgear.xml

public class UmbrellaProps : CompProperties
{
    public UmbrellaProps() : base(typeof(UmbrellaComp)) {}

    public UmbrellaProps(Type compClass) : base(compClass) {}

    // Set to true if the thing blocks sunlight. Weather-blocking clothing should
    // generally not block sunlight. (Consider, if a vampire's sunlight weakness
    // could be defeated merely by wearing a cowboy hat or coat, then it wouldn't
    // be much of a weakness.)
    public bool blocksSunlight;

    // List of WeatherDef names to be blocked by the thing. Weather-blocking clothing
    // should generally not block weather heavier than light rain. Maybe a rain coat
    // or other such purpose-made apparel are exceptions.
    public List<string> blocksWeather;

    // List of HediffDef names to apply to a pawn while the thing is being used to block
    // weather and/or sunlight. Clothing items have no hediffs by default. Umbrellas
    // have two debuffs by default, though these can be disabled in the settings:
    //
    // - Bumber_CombatDebuff
    // - Bumber_WorkDebuff
    //
    // When instantiating Hediffs from these HediffDefs, the Severity will be set to
    // the minSeverity of the 0th stage.
    public List<string> hediffs;

    // Set to true if this is clothing, not an umbrella. Players can toggle in the
    // settings whether clothing blocks sunlight and/or weather.
    public bool clothing;

    // If true, then the thing will be hidden when not being used to block weather
    // and/or sunlight. This should be true for actual umbrellas, and false for
    // clothing that blocks weather and/or sunlight, like hats or rain coats.
    public bool hideable;

    // Use of inheritance among apparel ThingDefs can break UmbrellaComp.
    //
    // Take the cowboy hat for example. We attach UmbrellaComp to Apparel_CowboyHat
    // so that it blocks light rain. Great, that works. But Apparel_BowlerHat uses
    // Apparel_CowboyHat as its parent, so it will also receive an UmbrellaComp,
    // which is not what we want.
    //
    // This property is a workaround. If this field is not empty, then UmbrellaComp
    // will remove itself from a thing's comps list if the thing's .def.defName does
    // not match the value of this property.
    //
    // So if we set this property to "Apparel_CowboyHat", then when a bowler spawns
    // and gets an UmbrellaComp, the comp has a way to notice that a bowler is not
    // a cowboy hat, and delete itself from the bowler's comps list.
    //
    // If the ThingDef will never be used as a parent for another def, then it is
    // safe to omit this property.
    public string defName;

    public bool BlocksSunlight => Settings.umbrellasBlockSun && blocksSunlight;

    public bool BlocksWeather(WeatherDef def)
    {
        return (!clothing || Settings.umbrellaClothing)
            && blocksWeather != null
            && blocksWeather.Contains(def.defName);
    }

    public bool IsForDef(string defName)
    {
        return string.IsNullOrWhiteSpace(this.defName) || defName == this.defName;
    }
}
