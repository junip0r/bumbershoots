namespace Bumbershoots.Reactor;

internal interface IReactor
{
    void Tick(int gameTick);

    void Reset();
}
