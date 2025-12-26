using BenchmarkDotNet.Attributes;
using Aoc2025.Days;
using Aoc2025.Benchmarks.Infrastructure;
using Aoc2025.Registry;

namespace Aoc2025.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(FastBenchmarkConfig))]
public class Day12Bench
{
    private string[] _input = [];
    private ISolution _solution = null!;

    [GlobalSetup]
    public void Setup()
    {
        DayLoader.LoadAll();
        _input = BenchmarkInput.LoadDay(12);
        DayRegistry.TryCreate(12, out _solution);
    }

    [Benchmark]
    public void SolveDay12()
    {
        _solution.SetInput(_input);
        _ = _solution.SolvePart1();
        _ = _solution.SolvePart2();
    }
}