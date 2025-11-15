using System.Runtime.CompilerServices;
using RimWorld;

namespace Bumbershoots.Ext.RimWorld;

internal static class ApparelExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static UmbrellaComp UmbrellaComp(this Apparel @this)
    {
        return @this.GetComp<UmbrellaComp>();
    }
}
