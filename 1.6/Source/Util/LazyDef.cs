using System;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Util;

public class LazyDef<T> where T : Def
{
    private readonly string defName;

    private readonly Lazy<T> value;

    public string DefName
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => defName;
    }

    public T Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => value.Value;
    }

    public bool Present
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => value.Value is not null;
    }

    public LazyDef(string defName)
    {
        this.defName = defName;
        value = new Lazy<T>(Lookup);
    }

    private T Lookup()
    {
        return DefDatabase<T>.GetNamedSilentFail(defName);
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
