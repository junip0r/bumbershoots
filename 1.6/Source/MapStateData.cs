using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Bumbershoots;

internal partial class MapState : MapComponent
{
#if DEBUG_MAPSTATE
    private const int CapSmall = 1;
    private const int CapLarge = 1;
#else
    private const int CapSmall = 8;
    private const int CapLarge = 128;
#endif

#if DEBUG_MAPSTATE
    private const int periodDebug = GenTicks.TicksPerRealSecond * 15;
    private int periodicTickDebugPrev = 0;
#endif

    private int? settingsPrev = null;
    private bool? sunlightPrev = null;
    private bool? weatherPrev = null;
    private readonly List<Pawn> thingsSpawned = new(CapLarge);
    private readonly List<Pawn> thingsDespawned = new(CapLarge);
    private readonly List<(Pawn, ThingDef)> apparelAdded = new(CapSmall);
    private readonly List<(Pawn, ThingDef)> apparelRemoved = new(CapSmall);
    private readonly List<IntVec3> positionsDirty = new(CapSmall);
    private readonly List<Pawn> pawnSteps = new(CapSmall);
    private readonly HashSet<Pawn> pawns = new(CapLarge);
    private readonly List<Pawn> pawnsDirty = new(CapSmall);
    private bool pawnsAllDirty = false;
    private readonly List<(string, Exception)> exceptions = [];
}
