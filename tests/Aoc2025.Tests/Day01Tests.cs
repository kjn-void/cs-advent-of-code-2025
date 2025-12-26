using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public class Day01Tests
{
    private static readonly string[] Example =
    {
        "L68",
        "L30",
        "R48",
        "L5",
        "R60",
        "L55",
        "L1",
        "L99",
        "R14",
        "L82",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day01();
        d.SetInput(Example);

        var got = d.SolvePart1();
        Assert.Equal("3", got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day01();
        d.SetInput(Example);

        var got = d.SolvePart2();
        Assert.Equal("6", got);
    }
}