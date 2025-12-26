using Aoc2025.Days;
using Xunit;

namespace Aoc2025.Tests;

public sealed class Day12Tests
{
    private const string Example = """
0:
###
##.
##.

1:
###
##.
.##

2:
.##
###
##.

3:
##.
###
##.

4:
###
#..
###

5:
###
.#.
###

4x4: 0 0 0 0 2 0
12x5: 1 0 1 0 2 2
12x5: 1 0 1 0 3 2
""";

    [Fact]
    public void Day12_Part1_Example()
    {
        var d = new Day12();
        d.SetInput(Example.Split('\n', StringSplitOptions.RemoveEmptyEntries));

        var result = d.SolvePart1();

        Assert.Equal("2", result);
    }
}