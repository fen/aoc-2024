namespace Aoc.Day19;

/// <summary>
/// To calculate the <b>number of distinct ways</b> to arrange towels for each design using the available patterns,
/// we need to modify the approach. Instead of just identifying if a design is possible,
/// we now need to track <b>all possible ways to create the design</b> and sum them up.
/// </summary>
public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        // Read all lines from the input file
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);

        // First line contains patterns
        var patterns = lines[0].Split(',').Select(x => x.Trim()).ToArray();
        // Skip the blank line and collect designs
        var designs = lines.Skip(2).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        var totalArrangements = CountAllArrangements(patterns, designs);

        return totalArrangements.ToString();
    }

    private static long CountAllArrangements(string[] patterns, string[] designs)
    {
        long totalArrangements = 0;

        foreach (var design in designs)
        {
            totalArrangements += CountWaysToFormDesign(patterns, design);
        }

        return totalArrangements;
    }

    private static long CountWaysToFormDesign(string[] patterns, string design)
    {
        var designLength = design.Length;

        // DP array: dp[i] stores the number of distinct ways to form the first `i` characters of the design
        var dp = new long[designLength + 1];
        dp[0] = 1; // Base case: one way to form an "empty design"

        for (var i = 1; i <= designLength; i++)
        {
            foreach (var pattern in patterns)
            {
                var patternLength = pattern.Length;

                if (i >= patternLength)
                {
                    if (design.Substring(i - patternLength, patternLength) == pattern) // Pattern matches
                    {
                        dp[i] += dp[i - patternLength];
                    }
                }
            }
        }

        return dp[designLength];
    }
}
