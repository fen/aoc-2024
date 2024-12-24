namespace Aoc.Day25;

public class Solution1 : ISolver
{
    public ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        // Read and parse input
        var input = File.ReadAllLines(inputFile.FullName);
        var locks = new List<int[]>();
        var keys = new List<int[]>();
        ParseInput(input, locks, keys);

        // Initialize total count of valid pairs
        int validPairs = 0;

        // Iterate through each lock and key
        foreach (var lockHeights in locks)
        {
            foreach (var keyHeights in keys)
            {
                if (IsCompatible(lockHeights, keyHeights, 7)) // Assume 7 rows for the schematics
                {
                    validPairs++;
                }
            }
        }

        // Return the total number of valid pairs
        return ValueTask.FromResult(validPairs.ToString());
    }

    private static void ParseInput(string[] input, List<int[]> locks, List<int[]> keys)
    {
        // Separate locks and keys based on their characteristics (locks: filled top row, keys: filled bottom row)
        var current = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (current.Count > 0)
                {
                    AddSchematic(current, locks, keys);
                    current.Clear();
                }
            }
            else
            {
                current.Add(line);
            }
        }

        if (current.Count > 0)
        {
            AddSchematic(current, locks, keys);
        }
    }

    private static void AddSchematic(List<string> schematic, List<int[]> locks, List<int[]> keys)
    {
        // Determine if the schematic is a lock or a key
        if (schematic[0].All(c => c == '#') && schematic[^1].All(c => c == '.'))
        {
            locks.Add(GetHeightsFromSchematic(schematic, false));
        }
        else if (schematic[0].All(c => c == '.') && schematic[^1].All(c => c == '#'))
        {
            keys.Add(GetHeightsFromSchematic(schematic, true));
        }
    }

    private static int[] GetHeightsFromSchematic(List<string> schematic, bool isKey)
    {
        // Convert the schematic into an array of column heights
        int columns = schematic[0].Length;
        int[] heights = new int[columns];

        for (int col = 0; col < columns; col++)
        {
            heights[col] = isKey
                ? schematic.Count - schematic.TakeWhile(row => row[col] == '.').Count()
                : schematic.TakeWhile(row => row[col] == '#').Count();
        }
        return heights;
    }

    private static bool IsCompatible(int[] lockHeights, int[] keyHeights, int totalHeight)
    {
        // Check if the key fits the lock without overlapping
        for (int i = 0; i < lockHeights.Length; i++)
        {
            if (lockHeights[i] + keyHeights[i] > totalHeight)
            {
                return false;
            }
        }
        return true;
    }
}
