using HarmonyLib;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Verse;

namespace Bumbershoots.Patch.Mod;

internal abstract class ModMethod
{
    internal abstract string MethodName { get; }
    internal abstract BindingFlags MethodFlags { get; }
    protected virtual Delegate Prefix => null;
    protected virtual Delegate Postfix => null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void E(ModMetaData mod, string msg)
    {
        Log.E($"{GetType()}: error patching mod {mod.Name}: {msg}");
    }

    internal void Patch(Harmony harmony, ModMetaData mod, ModType modType)
    {
        try
        {
            if (modType.Type is not Type type)
            {
                E(mod, $"type not found: {modType.TypeName}");
                return;
            }
            if (type.GetMethod(MethodName, MethodFlags) is not MethodInfo method)
            {
                E(mod, $"method not found: {modType.TypeName}.{MethodName}");
                return;
            }
            HarmonyMethod prefix = null, postfix = null;
            if (Prefix is Delegate pre) prefix = new(pre);
            if (Postfix is Delegate post) postfix = new(post);
            if (prefix is null && postfix is null)
            {
                E(mod, $"prefix and postfix delegates are both null: {modType.TypeName}.{MethodName}");
                return;
            }
            harmony.Patch(method, prefix: prefix, postfix: postfix);
        }
        catch (Exception e)
        {
            E(mod, $"{e}");
        }
    }
}
