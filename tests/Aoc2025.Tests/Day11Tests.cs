using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day11Tests
{
    private static string[] SplitLines(string s)
    {
        var raw = s.Split('\n');
        var list = new List<string>(raw.Length);

        foreach (var line in raw)
        {
            var trimmed = line.TrimEnd('\r');
            if (trimmed.Length == 0)
            {
                continue;
            }

            list.Add(trimmed);
        }

        return list.ToArray();
    }

    private const string ExamplePart1 = """
aaa: you hhh
you: bbb ccc
bbb: ddd eee
ccc: ddd eee fff
ddd: ggg
eee: out
fff: out
ggg: out
hhh: ccc fff iii
iii: out
""";

    private const string ExamplePart2 = """
svr: aaa bbb
aaa: fft
fft: ccc
bbb: tty
tty: ccc
ccc: ddd eee
ddd: hub
hub: fff
eee: dac
dac: fff
fff: ggg hhh
ggg: out
hhh: out
""";

    [Fact]
    public void Day11_Part1_Example()
    {
        var d = new Day11();
        d.SetInput(SplitLines(ExamplePart1));

        var result = d.SolvePart1();

        Assert.Equal("5", result);
    }

    [Fact]
    public void Day11_Part2_Example()
    {
        var d = new Day11();
        d.SetInput(SplitLines(ExamplePart2));

        var result = d.SolvePart2();

        Assert.Equal("2", result);
    }
}