namespace Bumbershoots.Reactor;

internal class PeriodicReactor(IReactor inner, int period) : IReactor
{
    private readonly IReactor inner = inner;
    protected readonly int period = period;

    private int prevTick = 0;

    public void Tick(int gameTick)
    {
        if (gameTick - prevTick >= period)
        {
            inner.Tick(gameTick);
            prevTick = gameTick;
        }
    }

    public void Reset()
    {
        prevTick = 0;
        inner.Reset();
    }
}
