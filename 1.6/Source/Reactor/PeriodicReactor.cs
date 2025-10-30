using System.Collections.Generic;

namespace Bumbershoots.Reactor;

internal class PeriodicReactor(int period, List<IReactor> reactors) : IReactor
{
    private readonly int period = period;
    private readonly List<IReactor> reactors = reactors;
    private int prevTick = 0;

    public void Tick(int gameTick)
    {
        if (gameTick - prevTick >= period)
        {
            reactors.Tick(gameTick);
            prevTick = gameTick;
        }
    }

    public void Reset()
    {
        prevTick = 0;
        reactors.Reset();
    }
}
