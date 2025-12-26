using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day11 : ISolution
{
    private Dictionary<string, List<string>> _adj = new();

    static Day11()
    {
        DayRegistry.Register(11, () => new Day11());
    }

    // -----------------------------------------------------------
    // Parsing
    // -----------------------------------------------------------

    public void SetInput(string[] lines)
    {
        _adj.Clear();

        foreach (var raw in lines)
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            // Format: "aaa: you hhh"
            var parts = line.Split(':', 2);
            var from = parts[0].Trim();

            var outs = new List<string>();
            if (parts.Length == 2)
            {
                var right = parts[1].Trim();
                if (right.Length != 0)
                {
                    foreach (var tok in right.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        outs.Add(tok);
                    }
                }
            }

            _adj[from] = outs;
        }
    }

    // -----------------------------------------------------------
    // Part 1 — count all paths from "you" to "out"
    // -----------------------------------------------------------

    public string SolvePart1()
    {
        if (_adj.Count == 0)
        {
            return "0";
        }

        var memo = new Dictionary<string, long>();
        var visiting = new HashSet<string>();

        var total = CountPathsFrom("you", memo, visiting);
        return total.ToString(CultureInfo.InvariantCulture);
    }

    private long CountPathsFrom(
        string node,
        Dictionary<string, long> memo,
        HashSet<string> visiting)
    {
        if (node == "out")
        {
            return 1;
        }

        if (memo.TryGetValue(node, out var cached))
        {
            return cached;
        }

        // Defensive cycle guard
        if (visiting.Contains(node))
        {
            return 0;
        }

        visiting.Add(node);

        long total = 0;
        if (_adj.TryGetValue(node, out var nexts))
        {
            foreach (var next in nexts)
            {
                total += CountPathsFrom(next, memo, visiting);
            }
        }

        visiting.Remove(node);
        memo[node] = total;
        return total;
    }

    // -----------------------------------------------------------
    // Part 2 — paths from "svr" to "out" visiting dac + fft
    // -----------------------------------------------------------

    public string SolvePart2()
    {
        if (_adj.Count == 0)
        {
            return "0";
        }

        var memo = new Dictionary<State, long>();

        var startMask = 0;
        if ("svr" == "dac")
        {
            startMask |= 1;
        }
        if ("svr" == "fft")
        {
            startMask |= 2;
        }

        var total = CountPathsWithRequired(
            "svr",
            startMask,
            memo);

        return total.ToString(CultureInfo.InvariantCulture);
    }

    private long CountPathsWithRequired(
        string node,
        int mask,
        Dictionary<State, long> memo)
    {
        var state = new State(node, mask);
        if (memo.TryGetValue(state, out var cached))
        {
            return cached;
        }

        if (node == "out")
        {
            return mask == 3 ? 1 : 0;
        }

        long total = 0;

        if (_adj.TryGetValue(node, out var nexts))
        {
            foreach (var next in nexts)
            {
                var nextMask = mask;

                if (next == "dac")
                {
                    nextMask |= 1;
                }
                if (next == "fft")
                {
                    nextMask |= 2;
                }

                total += CountPathsWithRequired(next, nextMask, memo);
            }
        }

        memo[state] = total;
        return total;
    }

    private readonly record struct State(string Node, int Mask);
}