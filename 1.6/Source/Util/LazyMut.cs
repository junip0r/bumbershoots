using System;
using System.Runtime.CompilerServices;

namespace Bumbershoots.Util;

internal class LazyMut<T>(Func<T> factory)
{
    private readonly Func<T> factory = factory;
    private System.Lazy<T> inner = new(factory);

    internal bool IsValueCreated
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => inner.IsValueCreated;
    }

    internal T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => inner.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        if (!inner.IsValueCreated) return;
        inner = new(factory);
    }
}
