using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day02 : ISolution
{
    // Inclusive ranges [L, R]
    private readonly List<(long L, long R)> _ranges = [];

    private static readonly long[] P10 = BuildPow10();

    static Day02()
    {
        DayRegistry.Register(2, () => new Day02());
    }

    public void SetInput(string[] lines)
    {
        _ranges.Clear();

        if (lines.Length == 0)
        {
            return;
        }

        var parts = lines[0].Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var bounds = part.Split('-');
            var start = long.Parse(bounds[0], CultureInfo.InvariantCulture);
            var end = long.Parse(bounds[1], CultureInfo.InvariantCulture);

            _ranges.Add((start, end));
        }
    }

    // ------------------------------------------------------------
    // Part 1
    // ------------------------------------------------------------
    public string SolvePart1()
    {
        long sum = 0;

        foreach (var (L, R) in _ranges)
        {
            var maxDigits = R.ToString(CultureInfo.InvariantCulture).Length;

            for (var k = 1; 2 * k <= maxDigits; k++)
            {
                var baseK = P10[k];
                var repFactor = baseK + 1;

                var dLo = P10[k - 1];
                var dHi = baseK - 1;

                var candMin = (L + repFactor - 1) / repFactor;
                var candMax = R / repFactor;

                if (candMin < dLo)
                {
                    candMin = dLo;
                }
                if (candMax > dHi)
                {
                    candMax = dHi;
                }
                if (candMin > candMax)
                {
                    continue;
                }

                for (var dd = candMin; dd <= candMax; dd++)
                {
                    sum += dd * repFactor;
                }
            }
        }

        return sum.ToString(CultureInfo.InvariantCulture);
    }

    // ------------------------------------------------------------
    // Part 2
    // ------------------------------------------------------------
    public string SolvePart2()
    {
        long total = 0;

        foreach (var (L, R) in _ranges)
        {
            var maxDigits = R.ToString(CultureInfo.InvariantCulture).Length;

            for (var totalDigits = 2; totalDigits <= maxDigits; totalDigits++)
            {
                var tenLen = P10[totalDigits];

                for (var m = 2; m <= totalDigits; m++)
                {
                    if (totalDigits % m != 0)
                    {
                        continue;
                    }

                    var k = totalDigits / m;

                    var baseK = P10[k];
                    var repFactor = (tenLen - 1) / (baseK - 1);

                    var dLo = P10[k - 1];
                    var dHi = baseK - 1;

                    var candMin = (L + repFactor - 1) / repFactor;
                    var candMax = R / repFactor;

                    if (candMin < dLo)
                    {
                        candMin = dLo;
                    }
                    if (candMax > dHi)
                    {
                        candMax = dHi;
                    }
                    if (candMin > candMax)
                    {
                        continue;
                    }

                    for (var dd = candMin; dd <= candMax; dd++)
                    {
                        var ds = dd.ToString(CultureInfo.InvariantCulture);

                        if (SmallestBlock(ds) != ds.Length)
                        {
                            continue;
                        }

                        total += dd * repFactor;
                    }
                }
            }
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }

    // ------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------
    private static long[] BuildPow10()
    {
        var t = new long[20];
        long x = 1;

        for (var i = 0; i < t.Length; i++)
        {
            t[i] = x;
            x *= 10;
        }

        return t;
    }

    private static int SmallestBlock(string s)
    {
        var n = s.Length;

        for (var k = 1; k <= n / 2; k++)
        {
            if (n % k != 0)
            {
                continue;
            }

            var block = s.AsSpan(0, k);
            var ok = true;

            for (var i = k; i < n; i += k)
            {
                if (!s.AsSpan(i, k).SequenceEqual(block))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                return k;
            }
        }

        return n;
    }
}