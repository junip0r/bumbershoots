using Prepatcher;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Bumbershoots.Ext.RimWorld;

public static class ApparelExt
{
    [PrepatcherField]
    [InjectComponent]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UmbrellaComp UmbrellaComp(this Apparel @this)
    {
        return @this.GetComp<UmbrellaComp>();
    }
}
