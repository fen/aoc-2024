namespace Aoc.Day20;

using static Day20Extensions;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var map = await inputFile.ReadAllLinesAsync();

        // Locate the start position ('S') on the map
        var startTilePosition = MapPosition.GetPositions(map)
            .Single(p => map[p.Y][p.X] == 'S');

        // Define possible movement directions
        MapPosition[] directions =
        [
            new (-1, 0), // Up
            new (1, 0), // Down
            new (0, 1), // Right
            new (0, -1) // Left
        ];

        return FindCheats(map, directions, startTilePosition, 100, 2).ToString();
    }
}

internal static class Day20Extensions
{
    public static int FindCheats(
        string[] map,
        MapPosition[] directions,
        MapPosition start,
        int picosecondsSaved,
        int cheatLength)
    {
        var result = 0;

        // Get the normal path from S to E
        var normalPath = FindPathToTarget(map, start, directions);

        // Loop over all pairs of positions on the normal path
        for (var i = 0; i < normalPath.Count; i++)
        {
            for (var j = i + 1; j < normalPath.Count; j++)
            {
                // Calculate Manhattan distance (cheat length)
                var length = Math.Abs(normalPath[j].X - normalPath[i].X) +
                             Math.Abs(normalPath[j].Y - normalPath[i].Y);

                // Compute time savings: time saved = (path steps - Manhattan distance)
                if (length <= cheatLength && j - i - length >= picosecondsSaved)
                {
                    ++result;
                }
            }
        }

        return result;
    }

    private static List<MapPosition> FindPathToTarget(string[] map, MapPosition start, MapPosition[] directions)
    {
        var queue = new Queue<List<MapPosition>>();
        queue.Enqueue([start]);

        while (queue.TryDequeue(out var currentPath))
        {
            var currentPosition = currentPath.Last();

            if (map[currentPosition.Y][currentPosition.X] == 'E') // Reached the end
                return currentPath;

            foreach (var direction in directions)
            {
                var nextPosition = currentPosition + direction;

                // Avoid revisiting previous position and walls
                if (!IsValidMove(nextPosition, currentPath, map))
                    continue;

                queue.Enqueue([..currentPath, nextPosition]);
            }
        }

        return []; // No path found

        static bool IsValidMove(MapPosition nextPosition, List<MapPosition> currentPath, string[] map)
        {
            var previousPosition = currentPath.Count > 1 ? currentPath[^2] : null;
            return nextPosition != previousPosition &&
                   map[nextPosition.Y][nextPosition.X] != '#';
        }
    }

    internal sealed record MapPosition(int X, int Y)
    {
        public static MapPosition operator +(MapPosition a, MapPosition b) => new(a.X + b.X, a.Y + b.Y);

        public static IEnumerable<MapPosition> GetPositions(string[] map)
        {
            return Enumerable.Range(1, map.Length - 2) // Avoid outer edges (walls)
                .SelectMany(_ => Enumerable.Range(1, map[0].Length - 2), (y, x) => new MapPosition(x, y));
        }
    }
}

