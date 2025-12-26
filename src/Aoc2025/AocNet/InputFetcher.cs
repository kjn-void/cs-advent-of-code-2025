using System.Net.Http;

namespace Aoc2025.AocNet;

public static class InputFetcher
{
    private const int Year = 2025;

    public static async Task<string[]> FetchInputAsync(int day, string session)
    {
        var url = $"https://adventofcode.com/{Year}/day/{day}/input";

        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add(
            "User-Agent",
            $"github.com-kjn-aoc{Year}/1.0 ({Environment.OSVersion})"
        );

        client.DefaultRequestHeaders.Add(
            "Cookie",
            $"session={session}"
        );

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var text = await response.Content.ReadAsStringAsync();
        return text.TrimEnd().Split('\n');
    }
}