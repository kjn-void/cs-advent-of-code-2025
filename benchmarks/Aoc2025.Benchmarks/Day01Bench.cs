using BenchmarkDotNet.Attributes;
using Aoc2025.Days;
using Aoc2025.Benchmarks.Infrastructure;
using Aoc2025.Registry;

namespace Aoc2025.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(FastBenchmarkConfig))]
public class Day01Bench
{
    private string[] _input = [];
    private ISolution _solution = null!;

    [GlobalSetup]
    public void Setup()
    {
        DayLoader.LoadAll();
        _input = BenchmarkInput.LoadDay(1);
        DayRegistry.TryCreate(1, out _solution);
    }

    [Benchmark]
    public void SolveDay01()
    {
        _solution.SetInput(_input);
        _ = _solution.SolvePart1();
        _ = _solution.SolvePart2();
    }
}