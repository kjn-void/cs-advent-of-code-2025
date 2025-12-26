using Aoc2025.Days;

namespace Aoc2025.Registry;

public static class Registry
{
    private static readonly Dictionary<int, Func<ISolution>> _days = [];

    public static void Register(int day, Func<ISolution> factory)
        => _days[day] = factory;

    public static ISolution Create(int day)
        => _days.TryGetValue(day, out var f)
            ? f()
            : throw new ArgumentException($"Day {day} not registered");
}