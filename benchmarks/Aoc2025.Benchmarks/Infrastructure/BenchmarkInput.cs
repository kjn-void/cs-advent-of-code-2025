namespace Aoc2025.Benchmarks.Infrastructure;

public static class BenchmarkInput
{
    public static string[] LoadDay(int day)
    {
        var fileName = $"day{day:D2}.txt";
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, "input", fileName);
            if (File.Exists(candidate))
            {
                return File.ReadAllLines(candidate);
            }

            dir = dir.Parent;
        }

        throw new FileNotFoundException(
            $"Could not find input/{fileName} by walking up from {AppContext.BaseDirectory}"
        );
    }
}