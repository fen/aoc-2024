namespace Aoc.Day12;

public class Solution2 : ISolver
{
    private int _rows;
    private int _cols;
    private bool[,] _visited = default!;
    private char[][] _map = default!;

    // Directions for BFS (up, down, left, right)
    private readonly int[] _dx = [-1, 1, 0, 0];
    private readonly int[] _dy = [0, 0, -1, 1];

    // Diagonal directions for corner checks
    private readonly (int, int)[] _corners = [(-1, 1), (1, 1), (1, -1), (-1, -1)];

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
                if (_visited[i, j])
                {
                    continue;
                }

                var plantType = _map[i][j];
                var (area, corners) = CalculateRegionWithCorners(i, j, plantType);
                totalPrice += area * corners;
            }
        }

        return totalPrice.ToString();
    }

    private (int area, int corners) CalculateRegionWithCorners(int x, int y, char plantType)
    {
        int area = 0, cornerCount = 0;
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));
        _visited[x, y] = true;

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            area++;

            // Check for convex and concave corners around this cell
            foreach (var (du, dv) in _corners)
            {
                cornerCount += CheckCorner(cx, cy, du, dv, plantType);
            }

            // Add all cardinal neighbors into the BFS
            for (var i = 0; i < 4; i++)
            {
                var nx = cx + _dx[i];
                var ny = cy + _dy[i];

                if (nx < 0 || ny < 0 || nx >= _rows || ny >= _cols || _map[nx][ny] != plantType)
                {
                    continue; // Boundary or different plant type
                }
                else if (!_visited[nx, ny])
                {
                    _visited[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return (area, cornerCount);
    }

    private int CheckCorner(int cx, int cy, int du, int dv, char plantType)
    {
        // Perpendicular neighbors
        var nx1 = cx + du;
        var ny1 = cy;

        var nx2 = cx;
        var ny2 = cy + dv;

        // Diagonal neighbor
        var nxDiag = cx + du;
        var nyDiag = cy + dv;

        var inBounds1 = IsInBounds(nx1, ny1) && _map[nx1][ny1] == plantType;
        var inBounds2 = IsInBounds(nx2, ny2) && _map[nx2][ny2] == plantType;
        var inBoundsDiag = IsInBounds(nxDiag, nyDiag) && _map[nxDiag][nyDiag] == plantType;

        return (inBounds1, inBounds2, inBoundsDiag) switch
        {
            // Convex corner: Both perpendicular neighbors are out of bounds or not part of the region
            (false, false, _) => 1,
            // Concave corner: Both perpendicular neighbors are part of the region, but the diagonal is out of bounds
            (true, true, false) => 1,
            _ => 0 // No corner detected
        };
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _rows && y < _cols;
    }
}
