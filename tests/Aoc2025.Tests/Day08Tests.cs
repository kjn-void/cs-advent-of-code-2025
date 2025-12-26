using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day08Tests
{
    private static readonly string[] Example =
    {
        "162,817,812",
        "57,618,57",
        "906,360,560",
        "592,479,940",
        "352,342,300",
        "466,668,158",
        "542,29,236",
        "431,825,988",
        "739,650,466",
        "52,470,668",
        "216,146,977",
        "819,987,18",
        "117,168,530",
        "805,96,715",
        "346,949,466",
        "970,615,88",
        "941,993,340",
        "862,61,35",
        "984,92,344",
        "425,690,689",
    };

    [Fact]
    public void Part1_Example()
    {
        var d = new Day08();
        d.SetInput(Example);

        var sizes = typeof(Day08)
            .GetMethod("RunConnections", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, new object[] { Example.Length, GetEdges(d), 10 }) as int[];

        var got = sizes![0] * sizes[1] * sizes[2];
        Assert.Equal(40, got);
    }

    [Fact]
    public void Part2_Example()
    {
        var d = new Day08();
        d.SetInput(Example);

        var result = d.SolvePart2();
        Assert.Equal("25272", result);
    }

    private static object GetEdges(Day08 d)
    {
        var f = typeof(Day08).GetField("_edges", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        return f.GetValue(d)!;
    }
}