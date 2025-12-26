using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day02Tests
{
    private static readonly string[] ExampleInput =
    {
        "11-22,95-115,998-1012,1188511880-1188511890,222220-222224," +
        "1698522-1698528,446443-446449,38593856-38593862,565653-565659," +
        "824824821-824824827,2121212118-2121212124"
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day02();
        d.SetInput(ExampleInput);

        var got = d.SolvePart1();
        var want = "1227775554";

        Assert.Equal(want, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day02();
        d.SetInput(ExampleInput);

        var got = d.SolvePart2();
        var want = "4174379265";

        Assert.Equal(want, got);
    }
}
