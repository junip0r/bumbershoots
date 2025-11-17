using System;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Util;

internal class Memo<T>(Func<T> factory)
{
    private readonly Func<T> factory = factory;
    private Lazy<T> lazy = new(factory);

    internal bool IsValueCreated
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => lazy.IsValueCreated;
    }

    internal T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => lazy.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        if (lazy.IsValueCreated)
        {
            lazy = new(factory);
        }
    }
}
