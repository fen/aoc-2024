namespace Aoc.Day20;

using static Day20Extensions;

public class Solution2 : ISolver
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

        return FindCheats(map, directions, startTilePosition, 100, 20).ToString();
    }
}
