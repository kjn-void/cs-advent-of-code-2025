using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day04Tests
{
    private static readonly string[] ExampleInput =
    {
        "..@@.@@@@.",
        "@@@.@.@.@@",
        "@@@@@.@.@@",
        "@.@@@@..@.",
        "@@.@@@@.@@",
        ".@@@@@@@.@",
        ".@.@.@.@@@",
        "@.@@@.@@@@",
        ".@@@@@@@@.",
        "@.@.@@@.@.",
    };

    [Fact]
    public void Part1_Example()
    {
        var day = new Day04();
        day.SetInput(ExampleInput);

        var got = day.SolvePart1();
        var want = "13";

        Assert.Equal(want, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var day = new Day04();
        day.SetInput(ExampleInput);

        var got = day.SolvePart2();
        var want = "43";

        Assert.Equal(want, got);
    }
}