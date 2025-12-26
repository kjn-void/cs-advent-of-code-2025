using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day05 : ISolution
{
    private readonly List<(long Start, long End)> _ranges = [];
    private readonly List<long> _ids = [];

    static Day05()
    {
        DayRegistry.Register(5, () => new Day05());
    }

    public void SetInput(string[] lines)
    {
        _ranges.Clear();
        _ids.Clear();

        var section = 0;

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                section++;
                continue;
            }

            if (section == 0)
            {
                var parts = line.Split('-');
                var start = long.Parse(parts[0], CultureInfo.InvariantCulture);
                var end = long.Parse(parts[1], CultureInfo.InvariantCulture);

                _ranges.Add((start, end));
            }
            else
            {
                var id = long.Parse(line, CultureInfo.InvariantCulture);
                _ids.Add(id);
            }
        }

        if (_ranges.Count == 0)
        {
            return;
        }

        // Sort and merge ranges
        _ranges.Sort(static (a, b) => a.Start.CompareTo(b.Start));

        var merged = new List<(long Start, long End)>(_ranges.Count);

        var curStart = _ranges[0].Start;
        var curEnd = _ranges[0].End;

        for (var i = 1; i < _ranges.Count; i++)
        {
            var (s, e) = _ranges[i];

            if (s <= curEnd)
            {
                if (e > curEnd)
                {
                    curEnd = e;
                }
            }
            else
            {
                merged.Add((curStart, curEnd));
                curStart = s;
                curEnd = e;
            }
        }

        merged.Add((curStart, curEnd));

        _ranges.Clear();
        _ranges.AddRange(merged);
    }

    public string SolvePart1()
    {
        var count = 0;

        foreach (var id in _ids)
        {
            if (IsFresh(id))
            {
                count++;
            }
        }

        return count.ToString(CultureInfo.InvariantCulture);
    }

    private bool IsFresh(long id)
    {
        var lo = 0;
        var hi = _ranges.Count - 1;

        while (lo <= hi)
        {
            var mid = (lo + hi) / 2;
            var (start, end) = _ranges[mid];

            if (id < start)
            {
                hi = mid - 1;
            }
            else if (id > end)
            {
                lo = mid + 1;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public string SolvePart2()
    {
        long total = 0;

        foreach (var (start, end) in _ranges)
        {
            total += (end - start + 1);
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }
}