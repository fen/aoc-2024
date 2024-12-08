namespace Aoc.Day02;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        int safeCount = 0;

        foreach (var line in lines)
        {
            var levels = line.Split(' ').Select(int.Parse).ToArray();

            if (AreLevelsSafe(levels))
            {
                safeCount++;
            }
        }

        return safeCount.ToString(CultureInfo.InvariantCulture);
    }

    private static bool AreLevelsSafe(int[] levels)
    {
        bool? isIncreasing = null;

        foreach (var (first, second) in levels.AdjacentPairs())
        {
            var difference = first - second;

            if (Math.Abs(difference) < 1 || Math.Abs(difference) > 3)
            {
                return false;
            }

            if (isIncreasing is null)
            {
                isIncreasing = difference > 0;
            }
            else if (isIncreasing != (difference > 0))
            {
                return false;
            }
        }

        return true;
    }
}
