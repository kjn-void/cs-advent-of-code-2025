using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day07 : ISolution
{
    private readonly List<string> _grid = [];
    private int _rows;
    private int _cols;
    private int _startCol;

    static Day07()
    {
        DayRegistry.Register(7, () => new Day07());
    }

    public void SetInput(string[] lines)
    {
        _grid.Clear();

        foreach (var line in lines)
        {
            _grid.Add(line);
        }

        var maxCols = _grid[0].Length;
        for (var i = 0; i < _grid.Count; i++)
        {
            if (_grid[i].Length < maxCols)
            {
                _grid[i] = _grid[i] + new string(' ', maxCols - _grid[i].Length);
            }
        }

        _rows = _grid.Count;
        _cols = maxCols;

        for (var c = 0; c < _cols; c++)
        {
            if (_grid[0][c] == 'S')
            {
                _startCol = c;
                return;
            }
        }
    }

    // -----------------------------------------------------------
    // Part 1 — Linear beam simulation
    // -----------------------------------------------------------

    public string SolvePart1()
    {
        var bufA = new bool[_cols];
        var bufB = new bool[_cols];

        var active = bufA;
        var next = bufB;

        active[_startCol] = true;
        var splitCount = 0;

        for (var r = 1; r < _rows; r++)
        {
            var row = _grid[r];
            Array.Clear(next);

            for (var c = 0; c < _cols; c++)
            {
                if (!active[c])
                {
                    continue;
                }

                if (row[c] == '^')
                {
                    splitCount++;

                    next[c - 1] = true;
                    next[c + 1] = true;
                }
                else
                {
                    next[c] = true;
                }
            }

            (next, active) = (active, next);
        }

        return splitCount.ToString(CultureInfo.InvariantCulture);
    }

    // -----------------------------------------------------------
    // Part 2 — Many-worlds timeline counting
    // -----------------------------------------------------------

    public string SolvePart2()
    {
        var bufA = new long[_cols];
        var bufB = new long[_cols];

        var timelines = bufA;
        var next = bufB;

        timelines[_startCol] = 1;

        for (var r = 1; r < _rows; r++)
        {
            var row = _grid[r];
            Array.Clear(next);

            for (var c = 0; c < _cols; c++)
            {
                var count = timelines[c];
                if (count == 0)
                {
                    continue;
                }

                if (row[c] == '^')
                {
                    next[c - 1] += count;
                    next[c + 1] += count;
                }
                else
                {
                    next[c] += count;
                }
            }

            (next, timelines) = (timelines, next);
        }

        long total = 0;
        for (var c = 0; c < _cols; c++)
        {
            total += timelines[c];
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }
}