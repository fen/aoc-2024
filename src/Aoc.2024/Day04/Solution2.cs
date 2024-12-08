namespace Aoc.Day04;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        return CountXMasOccurrences(lines).ToString();
    }

    private static int CountXMasOccurrences(string[] lines)
    {
        var count = 0;
        var rows = lines.Length;
        var cols = lines[0].Length;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col + 2 < cols; col++)
            {
                if (DownRight(lines, row, col) &&
                    DownLeft(lines, row, col + 2))
                {
                    count += 1;
                }
            }
        }

        return count;
    }


    private static bool DownRight(string[] lines, int row, int col)
    {
        return Find(lines, row, col, "MAS", 1, 1) ||
               Find(lines, row, col, "SAM", 1, 1);
    }

    private static bool DownLeft(string[] lines, int row, int col)
    {
        return Find(lines, row, col, "MAS", 1, -1) ||
               Find(lines, row, col, "SAM", 1, -1);
    }

    private static bool Find(string[] lines, int row, int col, string target, int rowDir, int colDir)
    {
        var targetLength = target.Length;
        var totalRows = lines.Length;
        var totalCols = lines[0].Length;

        for (var i = 0; i < targetLength; i++)
        {
            var currentRow = row + i * rowDir;
            var currentCol = col + i * colDir;

            if (IsOutOfBounds(currentRow, currentCol, totalRows, totalCols) ||
                lines[currentRow][currentCol] != target[i])
            {
                return false;
            }
        }

        return true;

        static bool IsOutOfBounds(int row, int col, int totalRows, int totalCols)
        {
            return row < 0 || row >= totalRows || col < 0 || col >= totalCols;
        }
    }
}
