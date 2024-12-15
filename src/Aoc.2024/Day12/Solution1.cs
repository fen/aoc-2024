namespace Aoc.Day12;

public class Solution1 : ISolver
{
    private int _rows;
    private int _cols;
    private bool[,] _visited = default!;
    private char[][] _map = default!;

    // Directions for DFS (up, down, left, right)
    private readonly int[] _dx = [-1, 1, 0, 0];
    private readonly int[] _dy = [0, 0, -1, 1];

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        // Read the input file and parse the garden map
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);
        _map = lines.Select(line => line.ToCharArray()).ToArray();

        _rows = _map.Length;
        _cols = _map[0].Length;
        _visited = new bool[_rows, _cols];

        var totalPrice = 0;

        // Main loop to process the map
        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _cols; j++)
            {
                if (!_visited[i, j])
                {
                    var plantType = _map[i][j];
                    var (area, perimeter) = CalculateRegion(i, j, plantType);
                    totalPrice += area * perimeter;
                }
            }
        }

        return totalPrice.ToString();
    }

    private (int area, int perimeter) CalculateRegion(int x, int y, char plantType)
    {
        int area = 0, perimeter = 0;
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));
        _visited[x, y] = true;

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            area++;

            for (var i = 0; i < 4; i++)
            {
                var nx = cx + _dx[i];
                var ny = cy + _dy[i];

                if (nx < 0 || ny < 0 || nx >= _rows || ny >= _cols || _map[nx][ny] != plantType)
                {
                    // Edge of the region, contributes to the perimeter
                    perimeter++;
                }
                else if (!_visited[nx, ny])
                {
                    _visited[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return (area, perimeter);
    }
}
