namespace Aoc.Day18;

/// 1. Initialize a 71x71 grid (all positions safe initially: `.`).
/// 2. For each byte position `(x, y)` from input, mark the grid position as corrupted: `#`.
/// 3. After 1024 bytes have fallen:
///    a. Use BFS to find the shortest path from (0, 0) to (70, 70), avoiding corrupted cells (`#`).
/// 4. Output the minimum number of steps to reach the exit, or indicate if it's unreachable.
public class Solution1 : ISolver
{
    private static readonly (int dx, int dy)[] Directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        const int GridSize = 71;
        var grid = InitializeGrid(GridSize);
        var input = (await inputFile.ReadAllLinesAsync())
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .ToArray();

        MarkBlockedCells(grid, input, 1024);
        var shortestPath = FindShortestPath(grid, GridSize);

        return shortestPath == -1 ? "Unreachable" : shortestPath.ToString();
    }

    private static char[,] InitializeGrid(int size)
    {
        var grid = new char[size, size];
        for (var i = 0; i < size; i++)
        for (var j = 0; j < size; j++)
            grid[i, j] = '.';

        return grid;
    }

    private static void MarkBlockedCells(char[,] grid, int[][] input, int limit)
    {
        for (var i = 0; i < Math.Min(input.Length, limit); i++)
        {
            var (x, y) = (input[i][0], input[i][1]);
            grid[x, y] = '#';
        }
    }

    private static int FindShortestPath(char[,] grid, int gridSize)
    {
        var queue = new Queue<(int x, int y, int steps)>();
        var visited = new HashSet<(int, int)> { (0, 0) };

        queue.Enqueue((0, 0, 0));

        while (queue.Count > 0)
        {
            var (x, y, steps) = queue.Dequeue();
            if (x == gridSize - 1 && y == gridSize - 1) return steps;

            foreach (var (dx, dy) in Directions)
            {
                int nx = x + dx, ny = y + dy;

                if (IsValidMove(nx, ny, gridSize, grid, visited))
                {
                    visited.Add((nx, ny));
                    queue.Enqueue((nx, ny, steps + 1));
                }
            }
        }

        return -1; // Unreachable
    }

    private static bool IsValidMove(int x, int y, int gridSize, char[,] grid, HashSet<(int, int)> visited)
    {
        return x >= 0 && y >= 0 && x < gridSize && y < gridSize &&
               grid[x, y] == '.' && !visited.Contains((x, y));
    }
}
