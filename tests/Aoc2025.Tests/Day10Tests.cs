using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day10Tests
{
    private const string Example = """
    [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
    [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
    """;

    [Fact]
    public void Part1_Example()
    {
        var d = new Day10();
        d.SetInput(Split(Example));
        Assert.Equal("7", d.SolvePart1());
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day10();
        d.SetInput(Split(Example));
        Assert.Equal("33", d.SolvePart2());
    }

    private static string[] Split(string s)
    {
        return s.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}