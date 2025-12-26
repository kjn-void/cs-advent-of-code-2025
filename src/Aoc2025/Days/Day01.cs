using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day01 : ISolution
{
    // Signed deltas:
    //   Rn => +n
    //   Ln => -n
    private readonly List<int> _moves = [];

    public void SetInput(string[] lines)
    {
        _moves.Clear();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
                continue;

            var dir = line[0];
            var val = int.Parse(line.AsSpan(1), CultureInfo.InvariantCulture);

            _moves.Add(dir == 'L' ? -val : val);
        }
    }

    private static int Mod100(int n)
    {
        n %= 100;
        if (n < 0) n += 100;
        return n;
    }

    public string SolvePart1()
    {
        var pos = 50;
        var countZero = 0;

        foreach (var delta in _moves)
        {
            pos = Mod100(pos + delta);
            if (pos == 0)
                countZero++;
        }

        return countZero.ToString(CultureInfo.InvariantCulture);
    }

    public string SolvePart2()
    {
        var pos = 50;
        var countZero = 0;

        foreach (var delta in _moves)
        {
            var step = delta < 0 ? -1 : 1;

            for (var moved = 0; moved != delta; moved += step)
            {
                pos += step;
                if (pos < 0) pos += 100;
                else if (pos >= 100) pos -= 100;

                if (pos == 0)
                    countZero++;
            }
        }

        return countZero.ToString(CultureInfo.InvariantCulture);
    }

    static Day01()
    {
        DayRegistry.Register(1, () => new Day01());
    }
}
