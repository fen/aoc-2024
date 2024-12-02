namespace Aoc.Day02;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var count = 0;
        foreach (var line in lines)
        {
            var levels = line.Split(' ').Select(int.Parse).ToArray();
            var modifiedLevels = new int[levels.Length - 1];

            if (IsSafe(levels))
            {
                count++;
            }
            else
            {
                for (var i = 0; i < levels.Length; i++)
                {
                    levels.AsSpan(0, i).CopyTo(modifiedLevels);
                    levels.AsSpan(i + 1).CopyTo(modifiedLevels.AsSpan(i));
                    if (IsSafe(modifiedLevels))
                    {
                        count++;
                        break;
                    }
                }
            }
        }

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
