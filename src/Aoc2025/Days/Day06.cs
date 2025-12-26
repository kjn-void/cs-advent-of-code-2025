using System.Globalization;
using Aoc2025.Registry;

namespace Aoc2025.Days;

public sealed class Day06 : ISolution
{
    private readonly List<string> _grid = [];
    private int _rows;
    private int _cols;

    static Day06()
    {
        DayRegistry.Register(6, () => new Day06());
    }

    public void SetInput(string[] lines)
    {
        _grid.Clear();

        foreach (var line in lines)
        {
            _grid.Add(line);
        }

        var maxCols = 0;
        foreach (var row in _grid)
        {
            if (row.Length > maxCols)
            {
                maxCols = row.Length;
            }
        }

        for (var i = 0; i < _grid.Count; i++)
        {
            if (_grid[i].Length < maxCols)
            {
                _grid[i] = _grid[i] + new string(' ', maxCols - _grid[i].Length);
            }
        }

        _rows = _grid.Count;
        _cols = maxCols;
    }

    // -----------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------

    private readonly struct Block
    {
        public readonly int Start;
        public readonly int End;

        public Block(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    private List<Block> FindBlocks()
    {
        var isBlank = new bool[_cols];

        for (var c = 0; c < _cols; c++)
        {
            var allSpace = true;

            for (var r = 0; r < _rows; r++)
            {
                if (_grid[r][c] != ' ')
                {
                    allSpace = false;
                    break;
                }
            }

            isBlank[c] = allSpace;
        }

        var blocks = new List<Block>(64);
        var inBlock = false;
        var start = 0;

        for (var c = 0; c < _cols; c++)
        {
            if (!isBlank[c])
            {
                if (!inBlock)
                {
                    inBlock = true;
                    start = c;
                }
            }
            else
            {
                if (inBlock)
                {
                    inBlock = false;
                    blocks.Add(new Block(start, c - 1));
                }
            }
        }

        if (inBlock)
        {
            blocks.Add(new Block(start, _cols - 1));
        }

        return blocks;
    }

    private char GetOperator(Block b)
    {
        var row = _grid[_rows - 1];

        for (var c = b.Start; c <= b.End; c++)
        {
            var ch = row[c];
            if (ch == '+' || ch == '*')
            {
                return ch;
            }
        }

        return '*';
    }

    // -----------------------------------------------------------
    // Number extractors
    // -----------------------------------------------------------

    private List<long> ExtractNumbersPart1(Block b)
    {
        var nums = new List<long>(_rows);

        for (var r = 0; r < _rows - 1; r++)
        {
            var slice = _grid[r].Substring(b.Start, b.End - b.Start + 1).Trim();
            var v = long.Parse(slice, CultureInfo.InvariantCulture);
            nums.Add(v);
        }

        return nums;
    }

    private List<long> ExtractNumbersPart2(Block b)
    {
        var nums = new List<long>(b.End - b.Start + 1);

        for (var c = b.Start; c <= b.End; c++)
        {
            Span<char> buffer = stackalloc char[_rows];
            var len = 0;

            for (var r = 0; r < _rows - 1; r++)
            {
                var ch = _grid[r][c];
                if (ch != ' ')
                {
                    buffer[len++] = ch;
                }
            }

            var v = long.Parse(buffer[..len], CultureInfo.InvariantCulture);
            nums.Add(v);
        }

        return nums;
    }

    // -----------------------------------------------------------
    // Shared block evaluation
    // -----------------------------------------------------------

    private long EvaluateBlocks(Func<Block, List<long>> extractor)
    {
        var blocks = FindBlocks();
        long total = 0;

        foreach (var b in blocks)
        {
            var nums = extractor(b);
            var op = GetOperator(b);
            total += EvalNumbers(nums, op);
        }

        return total;
    }

    private static long EvalNumbers(List<long> nums, char op)
    {
        if (op == '+')
        {
            long sum = 0;
            foreach (var n in nums)
            {
                sum += n;
            }
            return sum;
        }

        long prod = 1;
        foreach (var n in nums)
        {
            prod *= n;
        }
        return prod;
    }

    // -----------------------------------------------------------
    // Part 1
    // -----------------------------------------------------------

    public string SolvePart1()
    {
        var total = EvaluateBlocks(ExtractNumbersPart1);
        return total.ToString(CultureInfo.InvariantCulture);
    }

    // -----------------------------------------------------------
    // Part 2
    // -----------------------------------------------------------

    public string SolvePart2()
    {
        var total = EvaluateBlocks(ExtractNumbersPart2);
        return total.ToString(CultureInfo.InvariantCulture);
    }
}