using System;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

internal class LazyDef<T> where T : Def
{
    private readonly string defName;

    private readonly Lazy<T> def;

    internal string DefName
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => defName;
    }

    internal T Def
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => def.Value;
    }

    internal LazyDef(string defName)
    {
        this.defName = defName;
        def = new Lazy<T>(Lookup);
    }

    private T Lookup()
    {
        return DefDatabase<T>.GetNamed(defName);
    }

    public override int GetHashCode()
    {
        return defName.GetHashCode();
    }

    public override bool Equals(object o)
    {
        return o is LazyDef<T> && o.GetHashCode() == GetHashCode();
    }
}
