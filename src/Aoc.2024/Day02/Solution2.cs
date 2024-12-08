namespace Aoc.Day02;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var count = 0;
        foreach (var line in lines)
        {
            var levels = ParseLevels(line);
            if (IsSafe(levels) || CanBeSafeByRemovingOneLevel(levels))
            {
                count++;
            }
        }

        return count.ToString();
    }

    private static int[] ParseLevels(string line)
    {
        return line.Split(' ').Select(int.Parse).ToArray();
    }

    private static bool CanBeSafeByRemovingOneLevel(int[] levels)
    {
        var modifiedLevels = new int[levels.Length - 1];
        for (int i = 0; i < levels.Length; i++)
        {
            if (IsSafeWithRemoval(levels, modifiedLevels, i))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsSafeWithRemoval(int[] levels, int[] modifiedLevels, int index)
    {
        levels.AsSpan(0, index).CopyTo(modifiedLevels);
        levels.AsSpan(index + 1).CopyTo(modifiedLevels.AsSpan(index));
        return IsSafe(modifiedLevels);
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
