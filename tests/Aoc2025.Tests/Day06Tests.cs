using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day06Tests
{
    private static readonly string[] ExampleInput =
    {
        "123 328  51 64 ",
        " 45 64  387 23 ",
        "  6 98  215 314",
        "*   +   *   +  ",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day06();
        d.SetInput(ExampleInput);

        var got = d.SolvePart1();
        var want = "4277556";

        Assert.Equal(want, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day06();
        d.SetInput(ExampleInput);

        var got = d.SolvePart2();
        var want = "3263827";

        Assert.Equal(want, got);
    }
}