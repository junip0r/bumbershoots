using System;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Util;

public class LazyModMetaData
{
    private readonly string packageId;
    private readonly Lazy<ModMetaData> value;

    public string PackageId
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => packageId;
    }

    public ModMetaData Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => value.Value;
    }

    public bool Present
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => value.Value is not null;
    }

    public bool Active
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Present && value.Value.Active;
    }

    public LazyModMetaData(string packageId)
    {
        this.packageId = packageId;
        value = new Lazy<ModMetaData>(Lookup);
    }

    private ModMetaData Lookup()
    {
        return ModLister.GetModWithIdentifier(packageId);
    }

    public override int GetHashCode()
    {
        return packageId.GetHashCode();
    }

    public override bool Equals(object o)
    {
        return o is LazyModMetaData && o.GetHashCode() == GetHashCode();
    }
}
