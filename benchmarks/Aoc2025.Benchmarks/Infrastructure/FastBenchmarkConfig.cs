using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;

namespace Aoc2025.Benchmarks;

public sealed class FastBenchmarkConfig : ManualConfig
{
    public FastBenchmarkConfig()
    {
        AddJob(
            Job.ShortRun
                .WithWarmupCount(1)
                .WithIterationCount(3)
        );

        AddColumnProvider(DefaultColumnProviders.Instance);
        AddLogger(ConsoleLogger.Default);
        AddExporter(MarkdownExporter.GitHub);
    }
}