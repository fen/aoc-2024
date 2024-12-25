namespace Aoc.Day10;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();

        var map = new int[lines.Length, lines[0].Length];
        for (var r = 0; r < lines.Length; r++)
        {
            for (var c = 0; c < lines[0].Length; c++)
            {
                map[r, c] = lines[r][c] - '0';
            }
        }

        return CalculateTotalTrailheadScores(map).ToString();
    }

    private static readonly int[] dx = { -1, 1, 0, 0 };
    private static readonly int[] dy = { 0, 0, -1, 1 };

    private static int CalculateTotalTrailheadScores(int[,] map)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);
        var totalScore = 0;

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                if (map[r, c] == 0)
                {
                    totalScore += Bfs(map, r, c, rows, cols);
                }
            }
        }

        return totalScore;
    }

    private static int Bfs(int[,] map, int startingRow, int startingCol, int totalRows, int totalCols)
    {
        var queue = new Queue<(int row, int col, int height)>();
        var reachableNines = new HashSet<(int, int)>();
        var visitedCells = new bool[totalRows, totalCols];
        queue.Enqueue((startingRow, startingCol, 0));

        while (queue.Count > 0)
        {
            var (row, column, height) = queue.Dequeue();
            if (visitedCells[row, column]) continue;
            visitedCells[row, column] = true;

            for (var direction = 0; direction < 4; direction++)
            {
                var newRow = row + dx[direction];
                var newCol = column + dy[direction];

                if (newRow >= 0 && newRow < totalRows && newCol >= 0 && newCol < totalCols)
                {
                    if (!visitedCells[newRow, newCol] && map[newRow, newCol] == height + 1)
                    {
                        if (map[newRow, newCol] == 9)
                        {
                            reachableNines.Add((newRow, newCol));
                        }
                        else
                        {
                            queue.Enqueue((newRow, newCol, map[newRow, newCol]));
                        }
                    }
                }
            }
        }

        return reachableNines.Count;
    }
}
