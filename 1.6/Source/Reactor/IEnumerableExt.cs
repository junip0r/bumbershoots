using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bumbershoots.Ext.System.Collections.Generic;

namespace Bumbershoots.Reactor;

internal static class IEnumerableExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Tick(this IEnumerable<IReactor> reactors, int gameTick)
    {
        reactors.ForEach(r => r.Tick(gameTick));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Reset(this IEnumerable<IReactor> reactors)
    {
        reactors.ForEach(r => r.Reset());
    }
}
