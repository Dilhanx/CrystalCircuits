using System.Diagnostics;

namespace CrystalCircuits.Application.Controls.ModuleBoards;


class DeltaTime
{
    public double Time { get; private set; } = 0;
    public double Fps { get => 1.0 / Time; }
    double previousTime = 0;
    public DeltaTime()
    {
    }
    public void Calculate(TimeSpan elapsedTime)
    {
        Time = (elapsedTime.Ticks - previousTime) / Stopwatch.Frequency;
        previousTime = elapsedTime.Ticks;
    }
}