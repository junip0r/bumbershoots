using Bumbershoots.Ext.RimWorld;
using Bumbershoots.Ext.Verse;
using Bumbershoots.Ext.Verse.AI;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

#if DEBUG_MAPSTATE
using System.Text;
#endif

namespace Bumbershoots;

internal partial class MapState : MapComponent
{
    private void TickSettingsChanged()
    {
        var settingsCur = settingsPrev;
        try
        {
            settingsCur = Mod.Settings.GetHashCode();
            if (settingsPrev.HasValue && settingsCur != settingsPrev)
            {
#if DEBUG_MAPSTATE
                LogTick(nameof(TickSettingsChanged), "settings changed");
#endif
                Dirty();
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickSettingsChanged), e));
        }
        finally
        {
            settingsPrev = settingsCur;
        }
    }

    private void TickSunlightChanged()
    {
        var sunlightCur = sunlightPrev;
        try
        {
            sunlightCur = map.skyManager.IsUmbrellaSunlight();
            if (sunlightPrev.HasValue && sunlightCur != sunlightPrev)
            {
#if DEBUG_MAPSTATE
                LogTick(nameof(TickSunlightChanged), "sunlight changed");
#endif
                Dirty();
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickSunlightChanged), e));
        }
        finally
        {
            sunlightPrev = sunlightCur;
        }
    }

    private void TickWeatherChanged()
    {
        var weatherCur = weatherPrev;
        try
        {
            weatherCur = map.weatherManager.IsUmbrellaWeather();
            if (weatherPrev.HasValue && weatherCur != weatherPrev)
            {
#if DEBUG_MAPSTATE
                LogTick(nameof(TickWeatherChanged), "weather changed");
#endif
                Dirty();
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickWeatherChanged), e));
        }
        finally
        {
            weatherPrev = weatherCur;
        }
    }

    private void TickThingsSpawned()
    {
        if (thingsSpawned.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            LogTick(nameof(TickThingsSpawned), $"{thingsSpawned.Count} things spawned");
#endif
            foreach (var p in thingsSpawned)
            {
                try
                {
                    if (p.AnimalOrWildMan()) continue;
                    if (p.health.Dead) continue;
                    if (!p.apparel.IsWearingUmbrellaOrHat()) continue;
                    StartTrackingPawn(p);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickThingsSpawned), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickThingsSpawned), e));
        }
        finally
        {
            thingsSpawned.Clear();
        }
    }

    private void TickThingsDespawned()
    {
        if (thingsDespawned.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            LogTick(nameof(TickThingsDespawned), $"{thingsDespawned.Count} things despawned");
#endif
            foreach (var p in thingsDespawned)
            {
                try
                {
                    StopTrackingPawn(p);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickThingsDespawned), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickThingsDespawned), e));
        }
        finally
        {
            thingsDespawned.Clear();
        }
    }

    private void TickApparelAdded()
    {
        if (apparelAdded.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            LogTick(nameof(TickApparelAdded), $"{apparelAdded.Count} apparels added");
#endif
            foreach (var (p, apparel) in apparelAdded)
            {
                try
                {
                    if (!apparel.IsUmbrellaOrHat()) continue;
                    if (p.IsWildMan()) continue;
                    StartTrackingPawn(p);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickApparelAdded), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickApparelAdded), e));
        }
        finally
        {
            apparelAdded.Clear();
        }
    }

    private void TickApparelRemoved()
    {
        if (apparelRemoved.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            LogTick(nameof(TickApparelRemoved), $"{apparelRemoved.Count} apparels removed");
#endif
            foreach (var (p, apparel) in apparelRemoved)
            {
                try
                {
                    if (p.IsWildMan()) continue;
                    if (!apparel.IsUmbrellaOrHat()) continue;
                    if (p.apparel.IsWearingUmbrellaOrHat()) continue;
                    StopTrackingPawn(p);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickApparelRemoved), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickApparelRemoved), e));
        }
        finally
        {
            apparelRemoved.Clear();
        }
    }

    private void TickPositionsDirty()
    {
        if (positionsDirty.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            LogTick(nameof(TickPositionsDirty), $"{positionsDirty.Count} positions dirty");
#endif
            if (pawnsAllDirty) return;
            foreach (var p in pawns)
            {
                try
                {
                    if (positionsDirty.Contains(p.Position)) Dirty(p);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickPositionsDirty), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickPositionsDirty), e));
        }
        finally
        {
            positionsDirty.Clear();
        }
    }

    private void TickPawnSteps()
    {
        if (pawnSteps.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            // LogTick(nameof(TickPawnSteps), $"{pawnSteps.Count} pawn steps");
#endif
            if (pawnsAllDirty) return;
            var isUmbrellaWeather = map.weatherManager.IsUmbrellaWeather();
            foreach (var pather in pawnSteps)
            {
                try
                {
                    if (pather.Pawn() is not Pawn p) continue;
                    if (!pawns.Contains(p)) continue;
                    var last = new Lazy<IntVec3>(p.pather.LastCell);
                    var next = p.pather.nextCell;
                    var dirty = isUmbrellaWeather
                        && last.Value.Roofed(map) != next.Roofed(map);
                    dirty = dirty || map.skyManager.IsUmbrellaSunlight(p)
                        && last.Value.InSunlight(map) != next.InSunlight(map);
                    if (dirty) Dirty(p);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickPawnSteps), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickPawnSteps), e));
        }
        finally
        {
            pawnSteps.Clear();
        }
    }

    private void TickPawnsDirty()
    {
        if (pawns.Count == 0) return;
        ICollection<Pawn> dirty;
        if (pawnsAllDirty)
        {
            dirty = pawns;
            pawnsAllDirty = false;
        }
        else
        {
            dirty = pawnsDirty;
        }
        if (dirty.Count == 0) return;
        try
        {
#if DEBUG_MAPSTATE
            LogTick(nameof(TickPawnsDirty), $"{dirty.Count} pawns dirty");
#endif
            foreach (var p in dirty)
            {
                try
                {
                    p.PawnState().Update(this);
                }
                catch (Exception e)
                {
                    exceptions.Add((nameof(TickPawnsDirty), e));
                }
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickPawnsDirty), e));
        }
        finally
        {
            pawnsDirty.Clear();
        }
    }

    private void TickExceptions()
    {
        if (exceptions.Count == 0) return;
        try
        {
            LogErr(nameof(TickExceptions), $"{exceptions.Count} error(s) during tick");
            foreach (var (method, exception) in exceptions)
            {
                Log.W($"--- {nameof(MapState)}.{method}: {exception}");
            }
        }
        catch (Exception e)
        {
            LogErr(nameof(TickExceptions), $"the exception handler threw an exception. good times: {e}");
        }
        finally
        {
            exceptions.Clear();
        }
    }

#if DEBUG_MAPSTATE
    private void TickDebug()
    {
        try
        {
            var tick = GenTicks.TicksGame;
            if (tick - periodicTickDebugPrev < periodDebug) return;
            try
            {
                var s = new StringBuilder(256);
                s.Append("[");
                s.Append(map);
                s.Append("] | ");
                s.Append(typeof(MapState));
                s.Append(".");
                s.Append(nameof(TickDebug));
                s.Append(" | tick=");
                s.Append(tick);
                s.Append(" pawns=");
                s.Append(pawns.Count);
                s.Append(" pawnsDirty=");
                s.Append(pawnsAllDirty ? pawns.Count : pawnsDirty.Count);
                s.Append(" posDirty=");
                s.Append(positionsDirty.Count);
                s.Append(" pawnSteps=");
                s.Append(pawnSteps.Count);
                s.Append("/");
                s.Append(pawnSteps.Capacity);
                s.Append(" apparelAdd=");
                s.Append(apparelAdded.Count);
                s.Append("/");
                s.Append(apparelAdded.Capacity);
                s.Append(" apparelRem=");
                s.Append(apparelRemoved.Count);
                s.Append("/");
                s.Append(apparelRemoved.Capacity);
                s.Append(" thingsSpawn=");
                s.Append(thingsSpawned.Count);
                s.Append("/");
                s.Append(thingsSpawned.Capacity);
                s.Append(" thingsDespawn=");
                s.Append(thingsDespawned.Count);
                s.Append("/");
                s.Append(thingsDespawned.Capacity);
                s.Append(" sun=");
                s.Append(map.skyManager.IsUmbrellaSunlight() ? "yes" : "no");
                s.Append(" rain=");
                s.Append(map.weatherManager.IsUmbrellaWeather() ? "yes" : "no");
                Log.W(s.ToString());
            }
            finally
            {
                periodicTickDebugPrev = tick;
            }
        }
        catch (Exception e)
        {
            exceptions.Add((nameof(TickDebug), e));
        }
    }
#endif
}
