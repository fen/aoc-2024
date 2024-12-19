namespace Aoc.Day19;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        // Read all lines from the input file
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);

        // Parse input patterns and designs
        var patterns = lines[0].Split(',').Select(x => x.Trim()).ToArray(); // First line contains patterns
        var designs =
            lines.Skip(2).Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray(); // Skip the blank line and collect designs
        var result = CountPossibleDesigns(patterns, designs);

        return result.ToString();
    }

    public static int CountPossibleDesigns(string[] patterns, string[] designs)
    {
        var possibleCount = 0; // To count the number of possible designs

        foreach (var design in designs)
        {
            if (IsDesignPossible(patterns, design))
            {
                possibleCount++;
            }
        }

        return possibleCount;
    }

    private static bool IsDesignPossible(string[] patterns, string design)
    {
        var designLength = design.Length;
        // DP array: canForm[i] is true if we can form the first `i` characters of the design
        var canForm = new bool[designLength + 1];
        canForm[0] = true; // Base case

        for (var i = 1; i <= designLength; i++)
        {
            // Check every pattern to see if it matches the ending of the current substring
            foreach (var pattern in patterns)
            {
                var patternLength = pattern.Length;
                if (i >= patternLength && canForm[i - patternLength])
                {
                    // Check if the substring ending at `i` matches the current pattern
                    if (design.Substring(i - patternLength, patternLength) == pattern)
                    {
                        canForm[i] = true;
                        // No need to check further patterns for this position
                        break;
                    }
                }
            }
        }

        // Can we form the entire design?
        return canForm[designLength];
    }
}
