using System;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots;

internal class LazyModMetaData
{
    private readonly string packageId;
    private readonly Lazy<ModMetaData> modMetaData;

    internal string PackageId
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => packageId;
    }

    internal ModMetaData ModMetaData
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => modMetaData.Value;
    }

    internal bool Present
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ModMetaData is not null;
    }

    internal LazyModMetaData(string packageId)
    {
        this.packageId = packageId;
        modMetaData = new Lazy<ModMetaData>(Lookup);
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
