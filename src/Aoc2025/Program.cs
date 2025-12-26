using Aoc2025.IO;
using Aoc2025.Registry;
using Aoc2025.Days;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: aoc2025 <day> [day2 day3 ...]");
    Environment.Exit(1);
}

// Force static registration of all days once
DayLoader.LoadAll();

// Parse + validate days
var days = new List<int>();

foreach (var arg in args)
{
    if (!int.TryParse(arg, out var day) || day < 1 || day > 12)
    {
        Console.Error.WriteLine($"Invalid day: {arg}");
        continue;
    }
    days.Add(day);
}

if (days.Count == 0)
{
    Console.Error.WriteLine("No valid days specified.");
    Environment.Exit(1);
}

// Optional: run in ascending order
days.Sort();

foreach (var day in days)
{
    Console.WriteLine($"Day {day:D2}");

    // Load input (download or cached file)
    var lines = await InputLoader.LoadInputAsync(day);

    // Resolve solution
    if (!DayRegistry.TryCreate(day, out var solution))
    {
        Console.WriteLine("  Not implemented");
        Console.WriteLine();
        continue;
    }

    // Run solution
    solution.SetInput(lines);

    var part1 = solution.SolvePart1();
    var part2 = solution.SolvePart2();

    Console.WriteLine($"  Part 1: {part1}");
    Console.WriteLine($"  Part 2: {part2}");
    Console.WriteLine();
}