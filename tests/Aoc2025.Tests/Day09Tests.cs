using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day09Tests
{
    private static readonly string[] Example =
    {
        "7,1",
        "11,1",
        "11,7",
        "9,7",
        "9,5",
        "2,5",
        "2,3",
        "7,3",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day09();
        d.SetInput(Example);

        Assert.Equal("50", d.SolvePart1());
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day09();
        d.SetInput(Example);

        Assert.Equal("24", d.SolvePart2());
    }
}