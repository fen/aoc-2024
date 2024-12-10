namespace Aoc.Day10;

public class Solution2 : ISolver
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

        return CalculateTotalRatings(map).ToString();
    }

    private static int CalculateTotalRatings(int[,] map)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);
        var totalRating = 0;

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                if (map[r, c] == 0)
                {
                    totalRating += CountDistinctTrails(map, r, c, rows, cols);
                }
            }
        }

        return totalRating;
    }

    private static int CountDistinctTrails(int[,] map, int startR, int startC, int rows, int cols)
    {
        return Dfs(map, startR, startC, rows, cols, new bool[rows, cols]);
    }

    private const int TargetValue = 9;
    private static readonly int[] dx = { -1, 1, 0, 0 };
    private static readonly int[] dy = { 0, 0, -1, 1 };
    private static int Dfs(int[,] map, int x, int y, int rows, int cols, bool[,] visited)
    {
        if (map[x, y] == TargetValue) return 1;

        visited[x, y] = true;
        var pathCount = 0;

        for (var i = 0; i < 4; i++)
        {
            var nx = x + dx[i];
            var ny = y + dy[i];

            if (IsWithinBounds(nx, ny, rows, cols) && !visited[nx, ny] && IsValidPath(map, x, y, nx, ny))
            {
                pathCount += Dfs(map, nx, ny, rows, cols, visited);
            }
        }

        visited[x, y] = false;
        return pathCount;

        static bool IsWithinBounds(int x, int y, int rows, int cols)
        {
            return x >= 0 && x < rows && y >= 0 && y < cols;
        }

        static bool IsValidPath(int[,] map, int x, int y, int nx, int ny)
        {
            return map[nx, ny] == map[x, y] + 1;
        }
    }
}
