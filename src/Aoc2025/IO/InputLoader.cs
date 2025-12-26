using Aoc2025.AocNet;

namespace Aoc2025.IO;

public static class InputLoader
{
    public static async Task<string[]> LoadInputAsync(int day)
    {
        var path = Path.Combine("input", $"day{day:D2}.txt");

        if (File.Exists(path))
            return await File.ReadAllLinesAsync(path);

        if (Environment.GetEnvironmentVariable("AOC_ONLINE") != "1")
            throw new FileNotFoundException($"Missing input file: {path}");

        var session = Environment.GetEnvironmentVariable("AOC_SESSION")
            ?? throw new InvalidOperationException("AOC_SESSION not set");

        var lines = await InputFetcher.FetchInputAsync(day, session);

        Directory.CreateDirectory("input");
        await File.WriteAllLinesAsync(path, lines);

        return lines;
    }
}