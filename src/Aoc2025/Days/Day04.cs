using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day04 : ISolution
{
    private string[] _grid = Array.Empty<string>();
    private int _rows;
    private int _cols;

    // 8-connected neighborhood
    private static readonly (int dr, int dc)[] Dirs =
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };

    public void SetInput(string[] lines)
    {
        _grid = lines;
        _rows = lines.Length;
        _cols = _rows > 0 ? lines[0].Length : 0;
    }

    // ---------------------------------------------------------------------
    // Part 1
    // ---------------------------------------------------------------------

    public string SolvePart1()
    {
        if (_rows == 0 || _cols == 0)
        {
            return "0";
        }

        var total = 0;

        for (var r = 0; r < _rows; r++)
        {
            var row = _grid[r];

            for (var c = 0; c < _cols; c++)
            {
                if (row[c] != '@')
                {
                    continue;
                }

                if (CountAdjacent(r, c) < 4)
                {
                    total++;
                }
            }
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }

    private int CountAdjacent(int r, int c)
    {
        var count = 0;

        foreach (var (dr, dc) in Dirs)
        {
            var nr = r + dr;
            var nc = c + dc;

            if (nr >= 0 && nr < _rows
                && nc >= 0 && nc < _cols
                && _grid[nr][nc] == '@')
            {
                count++;
            }
        }

        return count;
    }

    // ---------------------------------------------------------------------
    // Part 2
    // ---------------------------------------------------------------------

    public string SolvePart2()
    {
        if (_rows == 0 || _cols == 0)
        {
            return "0";
        }

        var on = MakeBoolGrid();
        var deg = ComputeDegrees(on);

        var queue = new (int r, int c)[_rows * _cols];
        var qLen = 0;

        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _cols; c++)
            {
                if (on[r, c] && deg[r, c] < 4)
                {
                    queue[qLen++] = (r, c);
                }
            }
        }

        var removed = 0;
        var qp = 0;

        while (qp < qLen)
        {
            var (r, c) = queue[qp++];
            if (!on[r, c])
            {
                continue;
            }

            on[r, c] = false;
            removed++;

            foreach (var (dr, dc) in Dirs)
            {
                var nr = r + dr;
                var nc = c + dc;

                if (nr < 0 || nr >= _rows
                    || nc < 0 || nc >= _cols)
                {
                    continue;
                }

                if (!on[nr, nc])
                {
                    continue;
                }

                deg[nr, nc]--;

                if (deg[nr, nc] == 3)
                {
                    queue[qLen++] = (nr, nc);
                }
            }
        }

        return removed.ToString(CultureInfo.InvariantCulture);
    }

    private bool[,] MakeBoolGrid()
    {
        var on = new bool[_rows, _cols];

        for (var r = 0; r < _rows; r++)
        {
            var src = _grid[r];

            for (var c = 0; c < _cols; c++)
            {
                on[r, c] = src[c] == '@';
            }
        }

        return on;
    }

    private int[,] ComputeDegrees(bool[,] on)
    {
        var deg = new int[_rows, _cols];

        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _cols; c++)
            {
                if (!on[r, c])
                {
                    continue;
                }

                var cnt = 0;

                foreach (var (dr, dc) in Dirs)
                {
                    var nr = r + dr;
                    var nc = c + dc;

                    if (nr >= 0 && nr < _rows
                        && nc >= 0 && nc < _cols
                        && on[nr, nc])
                    {
                        cnt++;
                    }
                }

                deg[r, c] = cnt;
            }
        }

        return deg;
    }

    static Day04()
    {
        DayRegistry.Register(4, () => new Day04());
    }
}