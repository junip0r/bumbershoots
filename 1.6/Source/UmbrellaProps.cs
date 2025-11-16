using System.Collections.Generic;
using Verse;

namespace Bumbershoots;

// For usage examples, see:
//
// - Defs/Apparel_Umbrellas.xml
// - Patches/Apparel_Headgear.xml

public class UmbrellaProps() : CompProperties()
{
    // Set to true if the thing blocks sunlight. Weather-blocking clothing should
    // generally not block sunlight.
    public bool blocksSunlight;

    // List of WeatherDef names to be blocked by the thing. Weather-blocking clothing
    // should generally not block weather heavier than light rain. Maybe a rain coat
    // or other such purpose-made apparel are exceptions.
    public List<string> blocksWeather;

    // List of HediffDef names to apply to a pawn while the thing is being used to block
    // weather and/or sunlight. Weather-blocking clothing should generally not encumber.
    //
    // The defaults for umbrellas are:
    //
    // - Bumber_UmbrellaEncumbranceCombat
    // - Bumber_UmbrellaEncumbranceWork
    public List<string> encumbrances;

    // Set to true if this is clothing, not an umbrella. Players can toggle whether
    // clothing blocks sunlight and/or weather.
    public bool clothing;

    // If true, then the thing will be hidden when not being used to block weather
    // and/or sunlight. This should be true for actual umbrellas, and false for
    // clothing that blocks weather and/or sunlight, like hats or rain coats.
    //
    // Defaults to false, so may be omitted for apparel that does not hide.
    public bool hideable;

    // Use of inheritance among apparel ThingDefs can break UmbrellaComp.
    //
    // Take the cowboy hat for example. We attach UmbrellaComp to Apparel_CowboyHat
    // so that it blocks light rain. Great, that works. But Apparel_BowlerHat uses
    // Apparel_CowboyHat as its parent, so it will also receive an UmbrellaComp,
    // which is not what we want.
    //
    // This property is the workaround. If this field is not empty, then UmbrellaComp
    // will remove itself from a thing's comps list if the thing's .def.defName does
    // not match the value of this property.
    //
    // So if we set this property to "Apparel_CowboyHat", then when a bowler spawns
    // and gets an UmbrellaComp, the comp has a way to notice that a bowler is not
    // a cowboy hat, and delete itself from the bowler's comps list.
    //
    // There's probably a better way to account for the parent/child relationship,
    // but every approach I can think of has tradeoffs. This approach seems good
    // for compatibility in that the solution doesn't involve patching more things,
    // and this approach has almost zero runtime cost.
    //
    // If the ThingDef will never be used as a parent for another def, then it is
    // safe to omit this property.
    public string defName;
}
