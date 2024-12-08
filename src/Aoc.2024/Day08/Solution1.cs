namespace Aoc.Day08;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        return CalculateUniqueAntinodes(await inputFile.ReadCharMapAsync()).ToString();
    }

    private static int CalculateUniqueAntinodes(char[][] map)
    {
        var positions = CollectAntennaFrequencyPositions(map);
        var uniqueAntinodes = CalculateAntinodePositions(positions, map);

        return uniqueAntinodes.Count;
    }

    private static Dictionary<char, List<(int X, int Y)>> CollectAntennaFrequencyPositions(char[][] map)
    {
        var positions = new Dictionary<char, List<(int X, int Y)>>();

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                var c = map[y][x];
                if (char.IsLetterOrDigit(c))
                {
                    if (!positions.TryGetValue(c, out var value))
                    {
                        value = new List<(int, int)>();
                        positions[c] = value;
                    }

                    value.Add((x, y));
                }
            }
        }

        return positions;
    }

    private static HashSet<(int X, int Y)> CalculateAntinodePositions(
        Dictionary<char, List<(int X, int Y)>> positions, char[][] map)
    {
        var uniqueAntinodes = new HashSet<(int X, int Y)>();

        foreach (var posList in positions.Values)
        {
            for (var i = 0; i < posList.Count; i++)
            {
                for (var j = i + 1; j < posList.Count; j++)
                {
                    var pos1 = posList[i];
                    var pos2 = posList[j];

                    AddAntinodeIfValid(pos1, pos2, uniqueAntinodes, map);
                }
            }
        }

        return uniqueAntinodes;
    }

    private static void AddAntinodeIfValid(
        (int X, int Y) pos1, (int X, int Y) pos2, HashSet<(int X, int Y)> uniqueAntinodes, char[][] map)
    {
        var dx = pos2.X - pos1.X;
        var dy = pos2.Y - pos1.Y;

        var middle1 = (pos1.X - dx, pos1.Y - dy);
        var middle2 = (pos2.X + dx, pos2.Y + dy);

        if (IsValidPosition(middle1, map))
        {
            uniqueAntinodes.Add(middle1);
        }

        if (IsValidPosition(middle2, map))
        {
            uniqueAntinodes.Add(middle2);
        }
    }

    private static bool IsValidPosition((int X, int Y) position, char[][] map)
    {
        return position is { X: >= 0, Y: >= 0 } &&
               position.Y < map.Length && position.X < map[position.Y].Length;
    }
}
