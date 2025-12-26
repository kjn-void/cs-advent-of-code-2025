using BenchmarkDotNet.Attributes;
using Aoc2025.Days;
using Aoc2025.Benchmarks.Infrastructure;

namespace Aoc2025.Benchmarks;

[Config(typeof(FastBenchmarkConfig))]
[MemoryDiagnoser]
public class Day12Bench
{
    private readonly Day12 _day = new();
    private string[] _input = Array.Empty<string>();

    [GlobalSetup]
    public void Setup()
    {
        _input = BenchmarkInput.LoadDay(12);
    }

    [Benchmark]
    public void SetInput()
    {
        _day.SetInput(_input);
    }

    [Benchmark]
    public string Part1()
    {
        _day.SetInput(_input);
        return _day.SolvePart1();
    }

    [Benchmark]
    public string FullPipeline()
    {
        _day.SetInput(_input);
        return _day.SolvePart1();
    }
}