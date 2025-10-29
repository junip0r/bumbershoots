using System;
using System.Runtime.CompilerServices;

namespace Bumbershoots;

internal static class Exceptions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Exception ArgumentNull(string argName)
    {
        return new ArgumentNullException($"argument '{argName}' may not be null");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfArgumentNull(string argName, object arg)
    {
        if (arg is null) throw ArgumentNull(argName);
    }
}
