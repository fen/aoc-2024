namespace Aoc.Day02;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        int count = lines.Count(line =>
            IsSafe(line.Split(' ').Select(int.Parse).ToArray()) ||
            Enumerable.Range(0, line.Split(' ').Length)
                .Any(i => IsSafe(line.Split(' ').Where((_, index) => index != i)
                    .Select(int.Parse).ToArray())
                )
        );
        return count.ToString();
    }

    private static bool IsSafe(int[] levels)
    {
        bool? increasing = null;
        foreach (var (a, b) in levels.AdjacentPairs())
        {
            var c = a - b;
            if (Math.Abs(c) is < 1 or > 3)
            {
                return false;
            }

            if (increasing is null)
            {
                increasing = c > 0;
            }
            else if (increasing.Value != c > 0)
            {
                return false;
            }
        }

        return true;
    }
}
