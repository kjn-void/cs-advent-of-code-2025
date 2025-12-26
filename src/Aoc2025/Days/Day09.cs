using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day09 : ISolution
{
    private readonly List<Point> _reds = [];
    private readonly List<Edge> _edges = [];
    private readonly List<Edge> _vertEdges = [];

    static Day09()
    {
        DayRegistry.Register(9, () => new Day09());
    }

    // ----------------------------------------------------------
    // Input
    // ----------------------------------------------------------

    public void SetInput(string[] lines)
    {
        _reds.Clear();
        _edges.Clear();
        _vertEdges.Clear();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var parts = line.Split(',');
            var x = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var y = int.Parse(parts[1], CultureInfo.InvariantCulture);

            _reds.Add(new Point(x, y));
        }
    }

    // ----------------------------------------------------------
    // Part 1
    // ----------------------------------------------------------

    public string SolvePart1()
    {
        var best = MaxAreaInclusive(_reds);
        return best.ToString(CultureInfo.InvariantCulture);
    }

    private static long MaxAreaInclusive(List<Point> points)
    {
        var n = points.Count;
        if (n < 2)
        {
            return 0;
        }

        long best = 0;

        for (var i = 0; i < n; i++)
        {
            var a = points[i];

            for (var j = i + 1; j < n; j++)
            {
                var b = points[j];

                var dx = (long)Math.Abs(a.X - b.X) + 1;
                var dy = (long)Math.Abs(a.Y - b.Y) + 1;
                var area = dx * dy;

                if (area > best)
                {
                    best = area;
                }
            }
        }

        return best;
    }

    // ----------------------------------------------------------
    // Part 2
    // ----------------------------------------------------------

    public string SolvePart2()
    {
        var n = _reds.Count;
        if (n < 2)
        {
            return "0";
        }

        if (_edges.Count == 0)
        {
            BuildEdges();
        }

        var best = 0;

        for (var i = 0; i < n; i++)
        {
            var a = _reds[i];

            for (var j = i + 1; j < n; j++)
            {
                var b = _reds[j];

                var x1 = Math.Min(a.X, b.X);
                var x2 = Math.Max(a.X, b.X);
                var y1 = Math.Min(a.Y, b.Y);
                var y2 = Math.Max(a.Y, b.Y);

                var area = (x2 - x1 + 1) * (y2 - y1 + 1);
                if (area <= best)
                {
                    continue;
                }

                var c3 = new Point(x1, y2);
                var c4 = new Point(x2, y1);

                if (!PointInsideOrOn(c3)
                    || !PointInsideOrOn(c4))
                {
                    continue;
                }

                if (RectangleCutByPolygon(x1, y1, x2, y2))
                {
                    continue;
                }

                best = area;
            }
        }

        return best.ToString(CultureInfo.InvariantCulture);
    }

    // ----------------------------------------------------------
    // Polygon edges
    // ----------------------------------------------------------

    private void BuildEdges()
    {
        var n = _reds.Count;

        for (var i = 0; i < n; i++)
        {
            var a = _reds[i];
            var b = _reds[(i + 1) % n];

            var e = new Edge(a.X, a.Y, b.X, b.Y);

            if (a.Y == b.Y)
            {
                e.IsHorizontal = true;
                if (e.X1 > e.X2)
                {
                    (e.X1, e.X2) = (e.X2, e.X1);
                }
            }
            else
            {
                e.IsHorizontal = false;
                if (e.Y1 > e.Y2)
                {
                    (e.Y1, e.Y2) = (e.Y2, e.Y1);
                }

                _vertEdges.Add(e);
            }

            _edges.Add(e);
        }
    }

    // ----------------------------------------------------------
    // Point in polygon
    // ----------------------------------------------------------

    private bool PointInsideOrOn(Point p)
    {
        // Boundary allowed
        foreach (var e in _edges)
        {
            if (e.IsHorizontal)
            {
                if (p.Y == e.Y1
                    && p.X >= e.X1
                    && p.X <= e.X2)
                {
                    return true;
                }
            }
            else
            {
                if (p.X == e.X1
                    && p.Y >= e.Y1
                    && p.Y <= e.Y2)
                {
                    return true;
                }
            }
        }

        return PointInsideStrict(p);
    }

    // Integer-only vertical edge crossing
    private bool PointInsideStrict(Point p)
    {
        var crossings = 0;

        foreach (var e in _vertEdges)
        {
            if (e.X1 > p.X
                && e.Y1 <= p.Y
                && p.Y < e.Y2)
            {
                crossings++;
            }
        }

        return (crossings & 1) == 1;
    }

    // ----------------------------------------------------------
    // Rectangle cut test
    // ----------------------------------------------------------

    private bool RectangleCutByPolygon(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2 || y1 == y2)
        {
            return false;
        }

        foreach (var e in _edges)
        {
            if (e.IsHorizontal)
            {
                var y = e.Y1;
                if (y <= y1
                    || y >= y2)
                {
                    continue;
                }

                if (Math.Max(e.X1, x1) < Math.Min(e.X2, x2))
                {
                    return true;
                }
            }
            else
            {
                var x = e.X1;
                if (x <= x1
                    || x >= x2)
                {
                    continue;
                }

                if (Math.Max(e.Y1, y1) < Math.Min(e.Y2, y2))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // ----------------------------------------------------------
    // Internal structs
    // ----------------------------------------------------------

    private readonly struct Point(int x, int y)
    {
        public readonly int X = x;
        public readonly int Y = y;
    }

    private struct Edge(int x1, int y1, int x2, int y2)
    {
        public int X1 = x1;
        public int Y1 = y1;
        public int X2 = x2;
        public int Y2 = y2;
        public bool IsHorizontal = false;
    }
}