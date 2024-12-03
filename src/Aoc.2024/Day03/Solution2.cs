using System.Text.RegularExpressions;

namespace Aoc.Day03;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        string input = await File.ReadAllTextAsync(inputFile.FullName);

        var matches = Regex.Matches(input, @"(do\(\)|don't\(\)|mul\((\d+),(\d+)\))");

        long result = 0;
        var @do = true;
        foreach (Match match in matches)
        {
            var method = match.Groups[0].Value;

            if (method.StartsWith("mul") && @do)
            {
                var (_, _, first, second) = match.Groups;
                result += long.Parse(first.Value) * long.Parse(second.Value);
            }
            else if (method.StartsWith("don't"))
            {
                @do = false;
            }
            else if (method.StartsWith("do"))
            {
                @do = true;
            }
        }

        return result.ToString();
    }
}
