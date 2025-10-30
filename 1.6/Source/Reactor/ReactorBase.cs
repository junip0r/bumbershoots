using System;

namespace Bumbershoots.Reactor;

internal abstract class ReactorBase : IReactor
{
    protected abstract void OnTick(int gameTick);

    public void Tick(int gameTick)
    {
        try
        {
            OnTick(gameTick);
        }
        catch (Exception e)
        {
            Log.E($"{GetType()}.Tick(): {e}");
        }
    }

    protected virtual void OnReset()
    {
    }

    public void Reset()
    {
        try
        {
            OnReset();
        }
        catch (Exception e)
        {
            Log.E($"{GetType()}.Reset(): {e}");
        }
    }
}
