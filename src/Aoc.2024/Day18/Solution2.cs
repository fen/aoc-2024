namespace Aoc.Day18;

/// 1. Parse input into a list of byte fall positions.
/// 2. Initialize a 71x71 grid (`grid`) where all cells are safe (`.`).
/// 3. For each coordinate `(x, y)` in the input list:
///    a. Mark `grid[x, y] = '#'` to corrupt the memory at this position.
///    b. Use BFS to check if `(0,0)` can still reach `(70,70)`:
///       i. If reachable, continue with the next byte.
///       ii. If unreachable, return `(x, y)` as the first blocking byte.
/// 4. If the entire input is processed without blocking the path, return "No block."
public class Solution2 : ISolver
{
    private static readonly (int dx, int dy)[] _directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        const int gridSize = 71;
        var grid = InitializeGrid(gridSize);
        var input = (await inputFile.ReadAllLinesAsync())
            .Select(line => line.Split(',').Select(int.Parse).ToArray());

        foreach (var (x, y) in input.Select(coords => (coords[0], coords[1])))
        {
            grid[x, y] = '#';
            if (!IsPathReachable(grid, gridSize))
            {
                return $"{x},{y}";
            }
        }

        throw new UnreachableException("No block found.");
    }

    private static char[,] InitializeGrid(int size)
    {
        var grid = new char[size, size];
        for (var i = 0; i < size; i++)
        for (var j = 0; j < size; j++)
            grid[i, j] = '.';
        return grid;
    }

    private static bool IsPathReachable(char[,] grid, int gridSize)
    {
        var queue = new Queue<(int x, int y)>();
        var visited = new HashSet<(int, int)>();

        queue.Enqueue((0, 0));
        visited.Add((0, 0));

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            if ((x, y) == (gridSize - 1, gridSize - 1))
                return true;

            foreach (var (dx, dy) in _directions)
            {
                var next = (x + dx, y + dy);

                if (IsValidMove(next, gridSize, grid, visited))
                {
                    queue.Enqueue(next);
                    visited.Add(next);
                }
            }
        }

        return false;
    }

    private static bool IsValidMove((int x, int y) position, int gridSize, char[,] grid, HashSet<(int, int)> visited)
    {
        var (x, y) = position;
        return x >= 0 && y >= 0 && x < gridSize && y < gridSize &&
               grid[x, y] == '.' && !visited.Contains(position);
    }
}
