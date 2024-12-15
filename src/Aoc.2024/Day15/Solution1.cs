namespace Aoc.Day15;

public class Solution1 : ISolver
{
    private static readonly Dictionary<char, (int, int)> InstructionMap = new()
    {
        { '^', (0, -1) },
        { '>', (1, 0) },
        { 'v', (0, 1) },
        { '<', (-1, 0) }
    };

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);
        return Part1(lines.ToList()).ToString();
    }

    static int Part1(List<string> input)
    {
        var (grid, instructions) = ParseInput(input);
        var (boxPositions, currentPosition, walls) = InitializeGrid(grid);

        foreach (var instruction in instructions)
        {
            var direction = InstructionToCoordinate(instruction);
            (currentPosition, boxPositions) = TryMove(currentPosition, direction, boxPositions, walls);
        }

        return boxPositions.Sum(pos => (100 * pos.Item2) + pos.Item1);
    }

    static (string[] grid, string instructions) ParseInput(List<string> input)
    {
        var splitIndex = input.IndexOf("");
        return (input.Take(splitIndex).ToArray(), string.Concat(input.Skip(splitIndex + 1)));
    }

    static (HashSet<(int, int)> boxPositions, (int, int) currentPosition, HashSet<(int, int)> walls)
        InitializeGrid(string[] grid)
    {
        var boxPositions = new HashSet<(int, int)>();
        var walls = new HashSet<(int, int)>();
        var currentPosition = (0, 0);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                switch (grid[y][x])
                {
                    case '#': walls.Add((x, y)); break;
                    case 'O': boxPositions.Add((x, y)); break;
                    case '@': currentPosition = (x, y); break;
                }
            }
        }

        return (boxPositions, currentPosition, walls);
    }

    static (int, int) InstructionToCoordinate(char instruction) =>
        InstructionMap.GetValueOrDefault(instruction, (0, 0));

    static ((int, int), HashSet<(int, int)>) TryMove(
        (int, int) currentPosition,
        (int, int) direction,
        HashSet<(int, int)> boxes,
        HashSet<(int, int)> walls)
    {
        var nextPosition = (currentPosition.Item1 + direction.Item1, currentPosition.Item2 + direction.Item2);
        if (walls.Contains(nextPosition)) return (currentPosition, boxes);

        var movePosition = nextPosition;
        while (walls.Contains(movePosition) || boxes.Contains(movePosition))
        {
            if (walls.Contains(movePosition)) return (currentPosition, boxes);
            movePosition = (movePosition.Item1 + direction.Item1, movePosition.Item2 + direction.Item2);
        }

        if (boxes.Remove(nextPosition)) boxes.Add(movePosition);

        return (nextPosition, boxes);
    }
}
