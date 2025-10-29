using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Reactor;

internal static class ListExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Tick(this List<IReactor> reactors, int gameTick)
    {
        reactors.ForEach(r => r.Tick(gameTick));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Reset(this List<IReactor> reactors)
    {
        reactors.ForEach(r => r.Reset());
    }
}
