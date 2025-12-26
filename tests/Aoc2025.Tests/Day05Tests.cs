using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day05Tests
{
    private static readonly string[] ExampleInput =
    {
        "3-5",
        "10-14",
        "16-20",
        "12-18",
        "",
        "1",
        "5",
        "8",
        "11",
        "17",
        "32",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day05();
        d.SetInput(ExampleInput);

        var got = d.SolvePart1();
        var want = "3";

        Assert.Equal(want, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day05();
        d.SetInput(ExampleInput);

        var got = d.SolvePart2();
        var want = "14";

        Assert.Equal(want, got);
    }
}