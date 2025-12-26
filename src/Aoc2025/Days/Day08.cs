using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day08 : ISolution
{
    private readonly List<Vec3> _points = [];
    private Edge[] _edges = Array.Empty<Edge>();

    static Day08()
    {
        DayRegistry.Register(8, () => new Day08());
    }

    // -----------------------------------------------------------
    // Parsing
    // -----------------------------------------------------------

    public void SetInput(string[] lines)
    {
        _points.Clear();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var parts = line.Split(',');
            var x = long.Parse(parts[0], CultureInfo.InvariantCulture);
            var y = long.Parse(parts[1], CultureInfo.InvariantCulture);
            var z = long.Parse(parts[2], CultureInfo.InvariantCulture);

            _points.Add(new Vec3(x, y, z));
        }

        _edges = BuildSortedEdges(_points);
    }

    // -----------------------------------------------------------
    // Part 1
    // -----------------------------------------------------------

    public string SolvePart1()
    {
        var sizes = RunConnections(_points.Count, _edges, 1000);
        if (sizes.Length < 3)
        {
            return "0";
        }

        var result = sizes[0] * sizes[1] * sizes[2];
        return result.ToString(CultureInfo.InvariantCulture);
    }

    // -----------------------------------------------------------
    // Part 2
    // -----------------------------------------------------------

    public string SolvePart2()
    {
        if (_points.Count < 2)
        {
            return "0";
        }

        var (i, j) = RunUntilSingleCircuit(_points.Count, _edges);
        var xa = _points[i].X;
        var xb = _points[j].X;

        return (xa * xb).ToString(CultureInfo.InvariantCulture);
    }

    // -----------------------------------------------------------
    // Edge construction
    // -----------------------------------------------------------

    private static Edge[] BuildSortedEdges(List<Vec3> points)
    {
        var n = points.Count;
        if (n < 2)
        {
            return Array.Empty<Edge>();
        }

        var edgeCount = n * (n - 1) / 2;
        var edges = new Edge[edgeCount];
        var idx = 0;

        for (var i = 0; i < n; i++)
        {
            var a = points[i];
            for (var j = i + 1; j < n; j++)
            {
                var b = points[j];
                edges[idx++] = new Edge(
                    SquaredDist(a, b),
                    i,
                    j
                );
            }
        }

        Array.Sort(edges, static (a, b) => a.Dist2.CompareTo(b.Dist2));
        return edges;
    }

    private static long SquaredDist(Vec3 a, Vec3 b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        var dz = a.Z - b.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    // -----------------------------------------------------------
    // DSU helpers
    // -----------------------------------------------------------

    private static int[] RunConnections(int n, Edge[] edges, int k)
    {
        if (k > edges.Length)
        {
            k = edges.Length;
        }

        var uf = new Dsu(n);

        for (var i = 0; i < k; i++)
        {
            var e = edges[i];
            uf.Union(e.I, e.J);
        }

        var compSize = new Dictionary<int, int>(n);
        for (var i = 0; i < n; i++)
        {
            var r = uf.Find(i);
            compSize[r] = uf.Size[r];
        }

        var sizes = new int[compSize.Count];
        var idx = 0;
        foreach (var s in compSize.Values)
        {
            sizes[idx++] = s;
        }

        Array.Sort(sizes, static (a, b) => b.CompareTo(a));
        return sizes;
    }

    private static (int, int) RunUntilSingleCircuit(int n, Edge[] edges)
    {
        var uf = new Dsu(n);
        var components = n;

        var lastI = 0;
        var lastJ = 0;

        foreach (var e in edges)
        {
            if (uf.Union(e.I, e.J))
            {
                components--;
                lastI = e.I;
                lastJ = e.J;

                if (components == 1)
                {
                    break;
                }
            }
        }

        return (lastI, lastJ);
    }

    // -----------------------------------------------------------
    // Internal types
    // -----------------------------------------------------------

    private readonly struct Vec3
    {
        public readonly long X;
        public readonly long Y;
        public readonly long Z;

        public Vec3(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    private readonly struct Edge
    {
        public readonly long Dist2;
        public readonly int I;
        public readonly int J;

        public Edge(long dist2, int i, int j)
        {
            Dist2 = dist2;
            I = i;
            J = j;
        }
    }

    private sealed class Dsu
    {
        public readonly int[] Parent;
        public readonly int[] Size;

        public Dsu(int n)
        {
            Parent = new int[n];
            Size = new int[n];
            for (var i = 0; i < n; i++)
            {
                Parent[i] = i;
                Size[i] = 1;
            }
        }

        public int Find(int x)
        {
            while (Parent[x] != x)
            {
                Parent[x] = Parent[Parent[x]];
                x = Parent[x];
            }
            return x;
        }

        public bool Union(int a, int b)
        {
            var ra = Find(a);
            var rb = Find(b);

            if (ra == rb)
            {
                return false;
            }

            if (Size[ra] < Size[rb])
            {
                (ra, rb) = (rb, ra);
            }

            Parent[rb] = ra;
            Size[ra] += Size[rb];
            return true;
        }
    }
}