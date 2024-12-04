namespace Aoc.Day04;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        return CountXmasOccurrences(lines).ToString();
    }

     private static int CountXmasOccurrences(string[] lines)
    {
        var count = 0;
        var rows = lines.Length;
        var cols = lines[0].Length;
        string[] targets = ["XMAS", "SAMX"];

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                foreach (var target in targets)
                {
                    count += CountInDirection(lines, row, col, target, 0, 1);  // Horizontal
                    count += CountInDirection(lines, row, col, target, 1, 0);  // Vertical
                    count += CountInDirection(lines, row, col, target, 1, 1);  // Diagonal down-right
                    count += CountInDirection(lines, row, col, target, 1, -1); // Diagonal down-left
                }
            }
        }

        return count;
    }

    private static int CountInDirection(string[] lines, int row, int col, string target, int rowDir, int colDir)
    {
        var targetLength = target.Length;
        var rows = lines.Length;
        var cols = lines[0].Length;

        for (var i = 0; i < targetLength; i++)
        {
            var newRow = row + i * rowDir;
            var newCol = col + i * colDir;

            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || lines[newRow][newCol] != target[i])
            {
                return 0;
            }
        }

        return 1;
    }
}
