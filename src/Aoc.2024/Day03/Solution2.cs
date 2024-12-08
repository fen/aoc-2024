using System.Text.RegularExpressions;

namespace Aoc.Day03;

public partial class Solution2 : ISolver
{
    private readonly Regex Pattern = PatternRegex();

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        string input = await File.ReadAllTextAsync(inputFile.FullName);

        var matches = Pattern.Matches(input);

        long result = 0;
        var shouldMultiply = true;
        foreach (Match match in matches)
        {
            var method = match.Groups[0].Value;

            if (method.StartsWith("mul") && shouldMultiply)
            {
                result += MultiplyOperands(match.Groups);
            }
            else if (method.StartsWith("don't"))
            {
                shouldMultiply = false;
            }
            else if (method.StartsWith("do"))
            {
                shouldMultiply = true;
            }
        }

        return result.ToString();
    }

    private static long MultiplyOperands(GroupCollection groups)
    {
        var (_, _, first, second) = groups;
        return long.Parse(first.Value) * long.Parse(second.Value);
    }

    [GeneratedRegex(@"(do\(\)|don't\(\)|mul\((\d+),(\d+)\))")]
    private static partial Regex PatternRegex();
}
