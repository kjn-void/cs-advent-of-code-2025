using System.Globalization;
using System.Threading.Tasks;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day10 : ISolution
{
    private readonly List<MachineData> _machines = [];

    static Day10()
    {
        DayRegistry.Register(10, () => new Day10());
    }

    // ------------------------------------------------------------
    // Parsing
    // ------------------------------------------------------------

    public void SetInput(string[] lines)
    {
        _machines.Clear();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var lb = line.IndexOf('[');
            var rb = line.IndexOf(']');
            if (lb < 0 || rb < 0)
            {
                continue;
            }

            var lightStr = line[(lb + 1)..rb];
            var lights = new int[lightStr.Length];
            for (var i = 0; i < lightStr.Length; i++)
            {
                lights[i] = lightStr[i] == '#' ? 1 : 0;
            }

            var joltage = Array.Empty<int>();
            var cb = line.IndexOf('{');
            var ce = line.IndexOf('}');
            if (cb >= 0 && ce > cb)
            {
                joltage = ParseList(line[cb..(ce + 1)]);
            }

            var buttons = new List<int[]>();
            var mid = cb >= 0 ? line[(rb + 1)..cb] : line[(rb + 1)..];

            var idx = 0;
            while (idx < mid.Length)
            {
                var ps = mid.IndexOf('(', idx);
                if (ps < 0)
                {
                    break;
                }

                var pe = mid.IndexOf(')', ps);
                if (pe < 0)
                {
                    break;
                }

                buttons.Add(ParseList(mid[ps..(pe + 1)]));
                idx = pe + 1;
            }

            _machines.Add(new MachineData(lights, joltage, buttons));
        }
    }

    private static int[] ParseList(string s)
    {
        s = s.Trim();
        if (s.Length < 2)
        {
            return [];
        }

        s = s[1..^1];
        if (s.Length == 0)
        {
            return [];
        }

        var parts = s.Split(',');
        var result = new int[parts.Length];
        var count = 0;

        foreach (var p in parts)
        {
            if (int.TryParse(p.Trim(), out var v))
            {
                result[count++] = v;
            }
        }

        return result[..count];
    }

    // ------------------------------------------------------------
    // Part 1 (unchanged, GF(2))
    // ------------------------------------------------------------

    public string SolvePart1()
    {
        var total = 0;

        foreach (var m in _machines)
        {
            if (m.TargetLights.Length == 0)
            {
                continue;
            }

            total += SolveLights(m);
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }

    private static int SolveLights(MachineData m)
    {
        var n = m.TargetLights.Length;
        var mCount = m.Buttons.Count;

        var mat = new int[n, mCount + 1];

        for (var i = 0; i < n; i++)
        {
            mat[i, mCount] = m.TargetLights[i];
        }

        for (var j = 0; j < mCount; j++)
        {
            foreach (var idx in m.Buttons[j])
            {
                if (idx < n)
                {
                    mat[idx, j] = 1;
                }
            }
        }

        var pivotRow = 0;
        var pivotCol = new int[mCount];
        Array.Fill(pivotCol, -1);

        for (var c = 0; c < mCount && pivotRow < n; c++)
        {
            var sel = -1;
            for (var r = pivotRow; r < n; r++)
            {
                if (mat[r, c] == 1)
                {
                    sel = r;
                    break;
                }
            }

            if (sel < 0)
            {
                continue;
            }

            SwapRows(mat, pivotRow, sel);
            pivotCol[c] = pivotRow;

            for (var r = 0; r < n; r++)
            {
                if (r != pivotRow && mat[r, c] == 1)
                {
                    for (var k = c; k <= mCount; k++)
                    {
                        mat[r, k] ^= mat[pivotRow, k];
                    }
                }
            }

            pivotRow++;
        }

        var freeCount = 0;
        var free = new int[mCount];
        for (var c = 0; c < mCount; c++)
        {
            if (pivotCol[c] < 0)
            {
                free[freeCount++] = c;
            }
        }

        var best = int.MaxValue;
        var limit = 1 << freeCount;
        var x = new int[mCount];

        for (var mask = 0; mask < limit; mask++)
        {
            Array.Clear(x);

            for (var i = 0; i < freeCount; i++)
            {
                if (((mask >> i) & 1) != 0)
                {
                    x[free[i]] = 1;
                }
            }

            for (var c = mCount - 1; c >= 0; c--)
            {
                var r = pivotCol[c];
                if (r < 0)
                {
                    continue;
                }

                var v = mat[r, mCount];
                for (var k = c + 1; k < mCount; k++)
                {
                    v ^= mat[r, k] & x[k];
                }
                x[c] = v;
            }

            var sum = 0;
            for (var i = 0; i < mCount; i++)
            {
                sum += x[i];
            }

            if (sum < best)
            {
                best = sum;
            }
        }

        return best;
    }

    private static void SwapRows(int[,] mat, int a, int b)
    {
        if (a == b)
        {
            return;
        }

        var cols = mat.GetLength(1);
        for (var i = 0; i < cols; i++)
        {
            (mat[a, i], mat[b, i]) = (mat[b, i], mat[a, i]);
        }
    }

    // ------------------------------------------------------------
    // Part 2 (optimized + correct)
    // ------------------------------------------------------------

    public string SolvePart2()
    {
        if (_machines.Count == 0)
        {
            return "0";
        }

        var results = new int[_machines.Count];

        Parallel.For(0, _machines.Count, i =>
        {
            results[i] = SolveJoltage(_machines[i]);
        });

        var total = 0;
        for (var i = 0; i < results.Length; i++)
        {
            total += results[i];
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }

    private static int SolveJoltage(MachineData m)
    {
        var n = m.TargetJoltage.Length;
        var mCount = m.Buttons.Count;
        var cols = mCount + 1;

        var mat = new double[n * cols];

        static int Idx(int r, int c, int cols) => r * cols + c;

        for (var i = 0; i < n; i++)
        {
            mat[Idx(i, mCount, cols)] = m.TargetJoltage[i];
        }

        for (var j = 0; j < mCount; j++)
        {
            foreach (var idx in m.Buttons[j])
            {
                if (idx < n)
                {
                    mat[Idx(idx, j, cols)] = 1.0;
                }
            }
        }

        var pivotRow = 0;
        var pivotCol = new int[mCount];
        Array.Fill(pivotCol, -1);

        for (var c = 0; c < mCount && pivotRow < n; c++)
        {
            var sel = -1;
            for (var r = pivotRow; r < n; r++)
            {
                if (Math.Abs(mat[Idx(r, c, cols)]) > 1e-9)
                {
                    sel = r;
                    break;
                }
            }

            if (sel < 0)
            {
                continue;
            }

            if (sel != pivotRow)
            {
                for (var k = c; k <= mCount; k++)
                {
                    (mat[Idx(pivotRow, k, cols)], mat[Idx(sel, k, cols)])
                        = (mat[Idx(sel, k, cols)], mat[Idx(pivotRow, k, cols)]);
                }
            }

            var div = mat[Idx(pivotRow, c, cols)];
            for (var k = c; k <= mCount; k++)
            {
                mat[Idx(pivotRow, k, cols)] /= div;
            }

            for (var r = 0; r < n; r++)
            {
                if (r == pivotRow)
                {
                    continue;
                }

                var f = mat[Idx(r, c, cols)];
                if (Math.Abs(f) < 1e-9)
                {
                    continue;
                }

                for (var k = c; k <= mCount; k++)
                {
                    mat[Idx(r, k, cols)] -= f * mat[Idx(pivotRow, k, cols)];
                }
            }

            pivotCol[c] = pivotRow;
            pivotRow++;
        }

        var free = new int[mCount];
        var freeCount = 0;
        for (var c = 0; c < mCount; c++)
        {
            if (pivotCol[c] < 0)
            {
                free[freeCount++] = c;
            }
        }

        var bound = 0;
        foreach (var v in m.TargetJoltage)
        {
            if (v > bound)
            {
                bound = v;
            }
        }
        bound++;

        var best = int.MaxValue;
        var x = new double[mCount];

        void Dfs(int idx, int sum)
        {
            if (sum >= best)
            {
                return;
            }

            if (idx == freeCount)
            {
                var total = sum;

                for (var c = 0; c < mCount; c++)
                {
                    var r = pivotCol[c];
                    if (r < 0)
                    {
                        continue;
                    }

                    var v = mat[Idx(r, mCount, cols)];
                    for (var k = c + 1; k < mCount; k++)
                    {
                        v -= mat[Idx(r, k, cols)] * x[k];
                    }

                    if (v < -1e-6)
                    {
                        return;
                    }

                    var iv = Math.Round(v);
                    if (Math.Abs(v - iv) > 1e-6)
                    {
                        return;
                    }

                    total += (int)iv;
                }

                if (total < best)
                {
                    best = total;
                }

                return;
            }

            var cIdx = free[idx];
            for (var v = 0; v <= bound; v++)
            {
                x[cIdx] = v;
                Dfs(idx + 1, sum + v);
                if (sum + v >= best)
                {
                    break;
                }
            }
        }

        Dfs(0, 0);
        return best;
    }
}

// ------------------------------------------------------------
// Supporting record
// ------------------------------------------------------------

public sealed record MachineData(
    int[] TargetLights,
    int[] TargetJoltage,
    List<int[]> Buttons
);