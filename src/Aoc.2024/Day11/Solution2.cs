namespace Aoc.Day11;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        const int TotalBlinks = 75;

        var lines = await inputFile.ReadAllLinesAsync();
        var stones = lines[0].Split(' ').Select(long.Parse).ToList();

        var stoneCounts = new Dictionary<long, long>(); // Tracks counts of stones by value

        // Initialize the stoneCounts dictionary with each unique initial stone's count
        foreach (var stone in stones)
        {
            stoneCounts.TryAdd(stone, 0);
            stoneCounts[stone]++;
        }

        for (var blink = 0; blink < TotalBlinks; blink++)
        {
            stoneCounts = GetNewStoneCounts(stoneCounts);
        }

        return stoneCounts.Sum(pair => pair.Value).ToString();
    }

    /// Handles the transformation of stone counts for a single blink
    private static Dictionary<long, long> GetNewStoneCounts(Dictionary<long, long> currentStoneCounts)
    {
        var updatedStoneCounts = new Dictionary<long, long>();
        foreach (var (stoneValue, count) in currentStoneCounts)
        {
            // Rule 1: If stone is 0, it becomes 1.
            if (stoneValue == 0)
            {
                AddStone(updatedStoneCounts, 1, count);
            }
            // Rule 2: If it has an even number of digits, split into two stones
            else if (HasEvenNumberOfDigits(stoneValue))
            {
                var (leftHalf, rightHalf) = SplitStone(stoneValue);
                AddStone(updatedStoneCounts, leftHalf, count);
                AddStone(updatedStoneCounts, rightHalf, count);
            }
            // Rule 3: Otherwise, multiply stone by 2024
            else
            {
                AddStone(updatedStoneCounts, stoneValue * 2024, count);
            }
        }

        return updatedStoneCounts;
    }

    /// Check if the number of digits in the stone is even
    private static bool HasEvenNumberOfDigits(long number)
    {
        var digits = (int)Math.Floor(Math.Log10(number) + 1);
        return digits % 2 == 0;
    }

    /// Splits a stone into two smaller stones (left and right halves of digits)
    private static (long, long) SplitStone(long stone)
    {
        var digits = (int)Math.Floor(Math.Log10(stone) + 1);
        var mid = digits / 2;

        var divisor = (long)Math.Pow(10, digits - mid); // Calculate 10^(digits-mid)

        var left = stone / divisor; // Extract the left part
        var right = stone % divisor; // Extract the right part

        return (left, right);
    }

    /// Adds a stone count to the dictionary (handling new entries cleanly)
    private static void AddStone(Dictionary<long, long> counts, long stone, long countToAdd)
    {
        counts.TryAdd(stone, 0);
        counts[stone] += countToAdd;
    }
}
