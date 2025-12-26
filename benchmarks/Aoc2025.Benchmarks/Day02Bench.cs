using BenchmarkDotNet.Attributes;
using Aoc2025.Benchmarks.Infrastructure;
using Aoc2025.Days;

namespace Aoc2025.Benchmarks;

[Config(typeof(FastBenchmarkConfig))]
[MemoryDiagnoser]
public class Day02Bench
{
    private readonly Day02 _day = new();
    private string[] _input = [];

    [GlobalSetup]
    public void Setup()
    {
        _input = BenchmarkInput.LoadDay(2);
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
    public (string, string) FullPipeline()
    {
        _day.SetInput(_input);
        return (_day.SolvePart1(), _day.SolvePart2());
    }
}