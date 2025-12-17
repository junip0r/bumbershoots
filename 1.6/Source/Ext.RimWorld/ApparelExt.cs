using Prepatcher;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Bumbershoots.Ext.RimWorld;

internal static class ApparelExt
{
    [PrepatcherField]
    [InjectComponent]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static UmbrellaComp UmbrellaComp(this Apparel @this)
    {
        return @this.GetComp<UmbrellaComp>();
    }
}
