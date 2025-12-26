# Advent of Code 2025 — C# / .NET Edition

This repository contains a clean, extensible **C# / .NET** framework for solving **Advent of Code 2025**.  
Each day’s solution follows a simple interface, auto-registers itself, supports cached inputs, and is benchmarked using **BenchmarkDotNet**.

---

## 🧩 Solution Interface

Every puzzle day implements the following interface:

```csharp
public interface ISolution
{
    void SetInput(string[] lines);
    string SolvePart1();
    string SolvePart2();
}
```

Each day registers itself automatically via a static constructor.

---

## 📦 Project Structure

```
cs-advent-of-code-2025/
│
├── README.md
├── .gitignore
│
├── input/                      # Cached puzzle inputs (ignored by git)
│   └── .gitkeep
│
├── src/
│   └── Aoc2025/
│       ├── Aoc2025.csproj
│       ├── Program.cs          # CLI entry point
│       │
│       ├── Registry/
│       │   └── DayRegistry.cs  # Day registration + lookup
│       │
│       ├── Days/
│       │   ├── ISolution.cs
│       │   ├── Day01.cs
│       │   ├── Day02.cs
│       │   └── ... up to Day12.cs
│       │
│       └── Infrastructure/
│           └── InputLoader.cs  # Input loading + online fetch
│
├── benchmarks/
│   └── Aoc2025.Benchmarks/
│       ├── Aoc2025.Benchmarks.csproj
│       ├── Infrastructure/
│       │   └── FastBenchmarkConfig.cs
│       └── Day01Bench.cs
│           ... Day12Bench.cs
│
└── tests/
    └── Aoc2025.Tests/
```

---

## 🚀 Running Solutions

Run a **single day**:

```bash
dotnet run --project src/Aoc2025/Aoc2025.csproj -- 1
```

Run **multiple days**:

```bash
dotnet run --project src/Aoc2025/Aoc2025.csproj -- 1 4 5
```

Run **all available days**:

```bash
dotnet run --project src/Aoc2025/Aoc2025.csproj
```

The CLI accepts any number of day numbers (1–25).

---

## 🌐 Automatic Input Download (adventofcode.com)

The framework supports **automatic input download** using your personal Advent of Code session cookie.

Advent of Code does not provide OAuth or an API token. Authentication is done via a browser cookie:

```
session=YOUR_SESSION_TOKEN
```

---

## 🔑 How to Retrieve Your Session Token

1. Log in at: https://adventofcode.com/
2. Open Developer Tools
   - **Safari**: ⌥ Option + ⌘ Command + I
   - **Chrome**: F12 → Application tab
   - **Firefox**: F12 → Storage tab
3. Go to **Cookies → https://adventofcode.com**
4. Copy the value of the cookie named **`session`**

⚠️ **This token is private. Never commit it to Git.**

---

## 🧷 Enabling Automatic Download

Set the following environment variables:

### macOS / Linux
```bash
export AOC_SESSION="your-session-token"
export AOC_ONLINE=1
```

### Windows (PowerShell)
```powershell
$env:AOC_SESSION="your-session-token"
$env:AOC_ONLINE="1"
```

When enabled, running:

```bash
dotnet run --project src/Aoc2025/Aoc2025.csproj -- 1
```

will:
1. Download `https://adventofcode.com/2025/day/1/input`
2. Cache it as `input/day01.txt`
3. Use the cached file for future runs

If downloading fails, the program falls back to local input files.

---

## ⏱️ Benchmarks (BenchmarkDotNet)

Benchmarks are implemented using **BenchmarkDotNet** with a fast configuration.

### Run all benchmarks

```bash
dotnet run --project benchmarks/Aoc2025.Benchmarks -c Release
```

### Notes
- Benchmark classes **must not be sealed** (BenchmarkDotNet requirement)
- Inputs are loaded once per benchmark via `[GlobalSetup]`
- Includes:
  - `SetInput`
  - `SolvePart1`
  - `SolvePart2`
  - Full pipeline

---

## 📊 Benchmark Summary — Apple M4 (darwin/arm64)

| Day | Full (µs) |
| --- | --------- |
| 01  | 330       |
| 02  | 7         |
| 03  | 58        |
| 04  | 550       |
| 05  | 23        |
| 06  | 113       |
| 07  | 24        |
| 08  | 29_000    |
| 09  | 21_000    |
| 10  | 79_000    |
| 11  | 111       |
| 12  | 175       |

---

## 🛡️ Safety Notes

- `input/` is gitignored by default
- Session tokens are never stored on disk
- All downloads are opt-in via environment variables

---

Happy hacking & Merry Advent of Code! 🎄✨

