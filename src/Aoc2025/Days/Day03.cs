using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day03 : ISolution
{
    // Each bank is a list of digits (1–9)
    private readonly List<int[]> _banks = [];

    static Day03()
    {
        DayRegistry.Register(3, () => new Day03());
    }

    public void SetInput(string[] lines)
    {
        _banks.Clear();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var digits = new int[line.Length];
            for (var i = 0; i < line.Length; i++)
            {
                digits[i] = line[i] - '0';
            }

            _banks.Add(digits);
        }
    }

    // ------------------------------------------------------------
    // Part 1
    // ------------------------------------------------------------
    public string SolvePart1()
    {
        return MaxJoltage(2);
    }

    // ------------------------------------------------------------
    // Part 2
    // ------------------------------------------------------------
    public string SolvePart2()
    {
        return MaxJoltage(12);
    }

    // ------------------------------------------------------------
    // Core logic
    // ------------------------------------------------------------
    private string MaxJoltage(int pick)
    {
        long total = 0;

        foreach (var bank in _banks)
        {
            var n = bank.Length;

            var need = pick;
            var stack = new int[pick];
            var sp = 0; // stack pointer

            for (var i = 0; i < n; i++)
            {
                var dig = bank[i];
                var remaining = n - i;

                var canPop =
                    sp > 0
                    && remaining > need;

                while (canPop && stack[sp - 1] < dig)
                {
                    sp--;
                    need++;

                    canPop =
                        sp > 0
                        && remaining > need;
                }

                if (need > 0)
                {
                    stack[sp++] = dig;
                    need--;
                }
            }

            total += StackToNumber(stack, sp);
        }

        return total.ToString(CultureInfo.InvariantCulture);
    }

    private static long StackToNumber(int[] stack, int len)
    {
        long val = 0;

        for (var i = 0; i < len; i++)
        {
            val = val * 10 + stack[i];
        }

        return val;
    }
}