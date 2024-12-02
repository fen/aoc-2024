namespace Aoc.Day02;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();

        int count = 0;
        foreach (var line in lines)
        {
            var levels = line.Split(' ')
                .Select(int.Parse).ToArray();

            bool safe = true;
            bool? increasing = null;
            foreach (var t in levels.AdjacentPairs())
            {
                var (a, b) = t;
                var c  = a - b;
                if (Math.Abs(c) is >= 1 and <= 3)
                {
                    if (increasing is null)
                    {
                        increasing = c > 0;
                    }
                    else if (increasing.Value != c > 0)
                    {
                        safe = false;
                        break;
                    }

                    continue;
                }

                safe = false;
                break;
            }

            if (safe)
            {
                count++;
            }
        }

        return count.ToString(CultureInfo.InvariantCulture);
    }
}
