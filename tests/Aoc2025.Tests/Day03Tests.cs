using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day03Tests
{
    private static readonly string[] ExampleInput =
    {
        "987654321111111",
        "811111111111119",
        "234234234234278",
        "818181911112111",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day03();
        d.SetInput(ExampleInput);

        var got = d.SolvePart1();
        var want = "357";

        Assert.Equal(want, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day03();
        d.SetInput(ExampleInput);

        var got = d.SolvePart2();
        var want = "3121910778619";

        Assert.Equal(want, got);
    }
}