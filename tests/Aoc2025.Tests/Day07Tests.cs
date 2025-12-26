using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day07Tests
{
    private static readonly string[] ExampleInput =
    {
        ".......S.......",
        "...............",
        ".......^.......",
        "...............",
        "......^.^......",
        "...............",
        ".....^.^.^.....",
        "...............",
        "....^.^...^....",
        "...............",
        "...^.^...^.^...",
        "...............",
        "..^...^.....^..",
        "...............",
        ".^.^.^.^.^...^.",
        "...............",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day07();
        d.SetInput(ExampleInput);

        var got = d.SolvePart1();
        var want = "21";

        Assert.Equal(want, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day07();
        d.SetInput(ExampleInput);

        var got = d.SolvePart2();
        var want = "40";

        Assert.Equal(want, got);
    }
}