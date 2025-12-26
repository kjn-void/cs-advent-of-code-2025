using BenchmarkDotNet.Attributes;
using Aoc2025.Days;
using Aoc2025.Benchmarks.Infrastructure;

namespace Aoc2025.Benchmarks;

[Config(typeof(FastBenchmarkConfig))]
[MemoryDiagnoser]
public class Day04Bench
{
    private readonly Day04 _day = new();
    private string[] _input = [];

    [GlobalSetup]
    public void Setup()
    {
        _input = BenchmarkInput.LoadDay(4);
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
    public string Part2()
    {
        _day.SetInput(_input);
        return _day.SolvePart2();
    }

    [Benchmark]
    public (string Part1, string Part2) FullPipeline()
    {
        _day.SetInput(_input);
        return (_day.SolvePart1(), _day.SolvePart2());
    }
}