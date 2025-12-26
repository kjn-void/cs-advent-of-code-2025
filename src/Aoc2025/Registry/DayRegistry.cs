using Aoc2025.Days;

namespace Aoc2025.Registry;

public static class DayRegistry
{
    private static readonly Dictionary<int, Func<ISolution>> _map = [];

    public static void Register(int day, Func<ISolution> factory)
    {
        _map[day] = factory;
    }

    public static bool TryCreate(int day, out ISolution solution)
    {
        if (_map.TryGetValue(day, out var factory))
        {
            solution = factory();
            return true;
        }

        solution = null!;
        return false;
    }
}