using System;

namespace Bumbershoots.Reactor;

internal abstract class ReactorBase : IReactor
{
    protected abstract void DoTick(int gameTick);

    public void Tick(int gameTick)
    {
        try
        {
            DoTick(gameTick);
        }
        catch (Exception e)
        {
            Log.E($"{GetType()}.Tick(): {e}");
        }
    }

    protected virtual void DoReset()
    {
    }

    public void Reset()
    {
        try
        {
            DoReset();
        }
        catch (Exception e)
        {
            Log.E($"{GetType()}.Reset(): {e}");
        }
    }
}
