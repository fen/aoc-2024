using System.Text.RegularExpressions;

namespace Aoc.Day03;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        string input = await File.ReadAllTextAsync(inputFile.FullName);

        var matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)");

        long result = 0;
        foreach (Match match in matches)
        {
            if (match.Success)
            {
                var (_, first, second) = match.Groups;
                result += long.Parse(first.Value) * long.Parse(second.Value);
            }
        }

        return result.ToString();
    }
}
