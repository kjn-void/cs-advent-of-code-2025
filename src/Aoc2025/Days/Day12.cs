using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day12 : ISolution
{
    private readonly List<Shape> _shapes = [];
    private readonly List<Region> _regions = [];

    private const int SmallBoardMaxArea = 15 * 15;

    static Day12()
    {
        DayRegistry.Register(12, () => new Day12());
    }

    // ---------------------------------------------------------------------
    // Parsing
    // ---------------------------------------------------------------------

    public void SetInput(string[] lines)
    {
        _shapes.Clear();
        _regions.Clear();

        var i = 0;

        while (i < lines.Length && lines[i].Trim().Length == 0)
        {
            i++;
        }

        // ---------------- Shapes ----------------
        while (i < lines.Length)
        {
            var line = lines[i].Trim();
            if (line.Length == 0)
            {
                i++;
                continue;
            }

            if (IsRegionLine(line))
            {
                break;
            }

            if (!line.EndsWith(":", StringComparison.Ordinal))
            {
                i++;
                continue;
            }

            i++;

            var rows = new List<string>();
            while (i < lines.Length)
            {
                var s = lines[i].TrimEnd('\r', '\n');
                if (s.Trim().Length == 0)
                {
                    i++;
                    break;
                }

                var trimmed = s.Trim();
                if (trimmed.EndsWith(":", StringComparison.Ordinal)
                    || IsRegionLine(trimmed))
                {
                    break;
                }

                rows.Add(trimmed);
                i++;
            }

            if (rows.Count > 0)
            {
                _shapes.Add(BuildShape(rows));
            }
        }

        // ---------------- Regions ----------------
        while (i < lines.Length)
        {
            var line = lines[i].Trim();
            i++;

            if (line.Length == 0)
            {
                continue;
            }

            if (!IsRegionLine(line))
            {
                continue;
            }

            var parts = line.Split(':', 2);
            var dims = parts[0].Split('x', 2);

            var w = int.Parse(dims[0], CultureInfo.InvariantCulture);
            var h = int.Parse(dims[1], CultureInfo.InvariantCulture);

            var countsRaw = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var counts = new int[countsRaw.Length];

            for (var c = 0; c < countsRaw.Length; c++)
            {
                counts[c] = int.Parse(countsRaw[c], CultureInfo.InvariantCulture);
            }

            _regions.Add(new Region(w, h, counts));
        }
    }

    private static bool IsRegionLine(string s)
    {
        var idx = s.IndexOf(':');
        if (idx <= 0)
        {
            return false;
        }

        var head = s[..idx];
        var parts = head.Split('x');
        if (parts.Length != 2)
        {
            return false;
        }

        return int.TryParse(parts[0], out _)
               && int.TryParse(parts[1], out _);
    }

    // ---------------------------------------------------------------------
    // Part 1
    // ---------------------------------------------------------------------

    public string SolvePart1()
    {
        var valid = 0;

        foreach (var region in _regions)
        {
            if (RegionCanFit(region))
            {
                valid++;
            }
        }

        return valid.ToString(CultureInfo.InvariantCulture);
    }

    // ---------------------------------------------------------------------
    // Part 2 (unknown in puzzle – stub)
    // ---------------------------------------------------------------------

    public string SolvePart2()
    {
        return "0";
    }

    // ---------------------------------------------------------------------
    // Region solver
    // ---------------------------------------------------------------------

    private bool RegionCanFit(Region r)
    {
        var totalArea = 0;

        for (var i = 0; i < r.Counts.Length && i < _shapes.Count; i++)
        {
            totalArea += r.Counts[i] * _shapes[i].Area;
        }

        if (totalArea > r.Width * r.Height)
        {
            return false;
        }

        if (r.Width * r.Height <= SmallBoardMaxArea)
        {
            return CanTileSmall(r);
        }

        return true;
    }

    // ---------------------------------------------------------------------
    // Exact tiling (small boards)
    // ---------------------------------------------------------------------

    private bool CanTileSmall(Region r)
    {
        var w = r.Width;
        var h = r.Height;

        var placements = new List<int[]>[_shapes.Count];
        for (var i = 0; i < placements.Length; i++)
        {
            placements[i] = [];
        }

        for (var si = 0; si < _shapes.Count; si++)
        {
            foreach (var v in _shapes[si].Variants)
            {
                for (var y = 0; y <= h - v.Height; y++)
                {
                    for (var x = 0; x <= w - v.Width; x++)
                    {
                        var cells = new List<int>();
                        var ok = true;

                        foreach (var c in v.Cells)
                        {
                            var px = x + c.X;
                            var py = y + c.Y;
                            if (px < 0 || px >= w || py < 0 || py >= h)
                            {
                                ok = false;
                                break;
                            }
                            cells.Add(py * w + px);
                        }

                        if (ok)
                        {
                            placements[si].Add(cells.ToArray());
                        }
                    }
                }
            }
        }

        var board = new bool[w * h];
        var counts = (int[])r.Counts.Clone();

        return Backtrack(board, counts, placements);
    }

    private bool Backtrack(bool[] board, int[] counts, List<int[]>[] placements)
    {
        var remainingArea = 0;
        var done = true;

        for (var i = 0; i < counts.Length && i < _shapes.Count; i++)
        {
            if (counts[i] > 0)
            {
                done = false;
                remainingArea += counts[i] * _shapes[i].Area;
            }
        }

        if (done)
        {
            return true;
        }

        var free = 0;
        foreach (var b in board)
        {
            if (!b)
            {
                free++;
            }
        }

        if (remainingArea > free)
        {
            return false;
        }

        var bestShape = -1;
        var bestCount = int.MaxValue;

        for (var i = 0; i < counts.Length && i < placements.Length; i++)
        {
            if (counts[i] <= 0)
            {
                continue;
            }

            var feasible = 0;
            foreach (var pl in placements[i])
            {
                var ok = true;
                foreach (var idx in pl)
                {
                    if (board[idx])
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    feasible++;
                    if (feasible >= bestCount)
                    {
                        break;
                    }
                }
            }

            if (feasible == 0)
            {
                return false;
            }

            if (feasible < bestCount)
            {
                bestCount = feasible;
                bestShape = i;
            }
        }

        counts[bestShape]--;

        foreach (var pl in placements[bestShape])
        {
            var ok = true;
            foreach (var idx in pl)
            {
                if (board[idx])
                {
                    ok = false;
                    break;
                }
            }

            if (!ok)
            {
                continue;
            }

            foreach (var idx in pl)
            {
                board[idx] = true;
            }

            if (Backtrack(board, counts, placements))
            {
                return true;
            }

            foreach (var idx in pl)
            {
                board[idx] = false;
            }
        }

        counts[bestShape]++;
        return false;
    }

    // ---------------------------------------------------------------------
    // Shape construction
    // ---------------------------------------------------------------------

    private static Shape BuildShape(List<string> rows)
    {
        var h = rows.Count;
        var w = rows.Max(r => r.Length);

        var grid = new bool[h, w];
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < rows[y].Length; x++)
            {
                grid[y, x] = rows[y][x] == '#';
            }
        }

        var variants = new List<Variant>();
        var seen = new HashSet<string>();

        var g = grid;
        for (var r = 0; r < 4; r++)
        {
            if (r > 0)
            {
                g = Rotate(g);
            }

            for (var f = 0; f < 2; f++)
            {
                var gf = f == 0 ? g : Flip(g);
                var v = ToVariant(gf);
                if (v.Cells.Count == 0)
                {
                    continue;
                }

                var key = v.Key;
                if (seen.Add(key))
                {
                    variants.Add(v);
                }
            }
        }

        return new Shape(variants[0].Cells.Count, variants);
    }

    private static bool[,] Rotate(bool[,] g)
    {
        var h = g.GetLength(0);
        var w = g.GetLength(1);
        var r = new bool[w, h];

        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                r[x, h - 1 - y] = g[y, x];
            }
        }

        return r;
    }

    private static bool[,] Flip(bool[,] g)
    {
        var h = g.GetLength(0);
        var w = g.GetLength(1);
        var r = new bool[h, w];

        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                r[y, w - 1 - x] = g[y, x];
            }
        }

        return r;
    }

    private static Variant ToVariant(bool[,] g)
    {
        var h = g.GetLength(0);
        var w = g.GetLength(1);

        var minX = w;
        var minY = h;
        var maxX = -1;
        var maxY = -1;

        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                if (!g[y, x])
                {
                    continue;
                }

                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
            }
        }

        if (maxX < minX)
        {
            return Variant.Empty;
        }

        var cells = new List<Point>();
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                if (g[y, x])
                {
                    cells.Add(new Point(x - minX, y - minY));
                }
            }
        }

        return new Variant(maxX - minX + 1, maxY - minY + 1, cells);
    }
}

// -------------------------------------------------------------------------
// Supporting records
// -------------------------------------------------------------------------

public sealed record Point(int X, int Y);

public sealed record Variant(int Width, int Height, List<Point> Cells)
{
    public static readonly Variant Empty = new(0, 0, []);

    public string Key
    {
        get
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(Width).Append('x').Append(Height).Append(':');
            foreach (var c in Cells)
            {
                sb.Append(c.X).Append(',').Append(c.Y).Append(';');
            }
            return sb.ToString();
        }
    }
}

public sealed record Shape(int Area, List<Variant> Variants);

public sealed record Region(int Width, int Height, int[] Counts);