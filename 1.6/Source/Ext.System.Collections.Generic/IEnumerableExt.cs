using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Ext.System.Collections.Generic;

internal static class IEnumerableExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ForEach<T>(this IEnumerable<T> items, Action<T> fn)
    {
        foreach (var item in items)
        {
            fn(item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ForEach<P, Q>(this IEnumerable<P> items, Func<P, Q> fn)
    {
        foreach (var item in items)
        {
            fn(item);
        }
    }
}
