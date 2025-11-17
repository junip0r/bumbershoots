using Bumbershoots.Ext.Verse;
using Bumbershoots.Util;
using System;
using System.Reflection;
using Verse;

namespace Bumbershoots.Patch.Mod.Nps;

internal class NpsPatch : ModPatch
{
    internal override LazyModMetaData Mod => Mods.NaturesPrettySweet;
    protected override Type[] ModTypes => [typeof(Pawn_Tick)];

    private class Pawn_Tick : ModType
    {
        internal override string TypeName => "TKKN_NPS.Pawn_Tick";
        internal override Type[] ModMethods => [typeof(Pawn_Tick_MakeWet)];

        private class Pawn_Tick_MakeWet : ModMethod
        {
            internal override string MethodName => "MakeWet";
            internal override BindingFlags MethodFlags => BindingFlags.Static | BindingFlags.NonPublic;
            protected override Delegate Prefix => MakeWet;

            private static readonly LazyDef<HediffDef> TKKN_Wetness = new("TKKN_Wetness");
            private const string TKKN_Wet = "TKKN_Wet";

            private static bool MakeWet(Pawn pawn)
            {
                // Block soaking wet from weather if carrying an appropriate umbrella,
                // but umbrellas will not block soaking wet from terrain.

                if (!NpsSettings.AllowPawnsToGetWet.GetValueOrDefault()) return false;
                if (TKKN_Wetness.Value is not HediffDef wetness) return false;
                if (pawn.health.hediffSet.HasHediff(wetness)) return false;
                if (pawn.Map is not Map m) return false;
                var pos = pawn.Position;
                var makeWet = m.weatherManager.CurWeatherLerped.IsRain()
                    && !pos.Roofed(m)
                    && (pawn.PawnComp() is not PawnComp pawnComp
                        || pawnComp.umbrellaComp is not UmbrellaComp umbrellaComp
                        || !umbrellaComp.blockingWeather);
                makeWet = makeWet || GridsUtility.GetTerrain(pos, m).HasTag(TKKN_Wet);
                if (!makeWet) return false;
                var h = HediffMaker.MakeHediff(TKKN_Wetness.Value, pawn);
                h.Severity = 0f;
                pawn.health.AddHediff(h);
                return false;
            }
        }
    }
}
