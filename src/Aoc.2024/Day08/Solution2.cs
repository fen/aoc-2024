namespace Aoc.Day08;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        return CalculateUniqueAntinodes(await inputFile.ReadCharMapAsync()).ToString();
    }

    private static int CalculateUniqueAntinodes(char[][] map)
    {
        // Collect positions of each antenna frequency
        var positions = CollectPositions(map);
        var uniqueAntinodes = new HashSet<(int X, int Y)>();

        // For each frequency, calculate antinodes
        foreach (var posList in positions.Values)
        {
            ProcessFrequency(posList, uniqueAntinodes, map.Length, map[0].Length);
        }

        return uniqueAntinodes.Count;
    }

    private static void ProcessFrequency(List<(int X, int Y)> posList, HashSet<(int X, int Y)> uniqueAntinodes, int height, int width)
    {
        if (posList.Count < 2)
            return;

        // Add each antenna as an antinode
        uniqueAntinodes.UnionWith(posList);

        // Compute and add collinear antinodes
        AddCollinearAntinodes(posList, uniqueAntinodes, height, width);
    }

    private static Dictionary<char, List<(int X, int Y)>> CollectPositions(char[][] map)
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

    private static void AddCollinearAntinodes(List<(int X, int Y)> posList, HashSet<(int X, int Y)> uniqueAntinodes,
        int height, int width)
    {
        for (var i = 0; i < posList.Count; i++)
        {
            for (var j = i + 1; j < posList.Count; j++)
            {
                var start = posList[i];
                var end = posList[j];

                // Get all possible collinear points on the line through pos1 and pos2
                MarkCollinearPoints(start, end, uniqueAntinodes, height, width);
            }
        }
    }

    private static void MarkCollinearPoints((int X, int Y) start, (int X, int Y) end,
        HashSet<(int X, int Y)> uniqueAntinodes, int height, int width)
    {
        var dX = end.X - start.X;
        var dY = end.Y - start.Y;

        var steps = Gcd(Math.Abs(dX), Math.Abs(dY));

        var stepX = dX / steps;
        var stepY = dY / steps;

        void MarkPointsInDirection(int startMultiplier, int stepMultiplier)
        {
            for (var multiplier = startMultiplier;; multiplier += stepMultiplier)
            {
                var x = start.X + stepX * multiplier;
                var y = start.Y + stepY * multiplier;

                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    uniqueAntinodes.Add((x, y));
                }
                else break; // Break when stepping beyond bounds
            }
        }

        // Extend indefinitely in both directions
        MarkPointsInDirection(0, 1); // Positive direction
        MarkPointsInDirection(-1, -1); // Negative direction
    }

    private static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return a;
    }
}
