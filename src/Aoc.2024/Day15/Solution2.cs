namespace Aoc.Day15;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);
        return CalculateResult(lines.ToList()).ToString();
    }

    private static int CalculateResult(List<string> input)
    {
        var inputSplit = string.Join("\n", input).Split(["\n\n"], StringSplitOptions.None);

        // Parse the grid and moves
        var grid = inputSplit[0]
            .Replace("#", "##")
            .Replace("O", "[]")
            .Replace(".", "..")
            .Replace("@", "@.")
            .Split('\n');

        var moves = inputSplit[1].Replace("\n", "");

        var boxPositions = new HashSet<(int, int)>();
        var wallPositions = new HashSet<(int, int)>();
        var robotPosition = (0, 0);

        // Initialize game positions
        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                switch (grid[y][x])
                {
                    case '[':
                        boxPositions.Add((y, x));
                        break;
                    case '#':
                        wallPositions.Add((y, x));
                        break;
                    case '@':
                        robotPosition = (y, x);
                        break;
                }
            }
        }

        // Process each move in the instructions
        foreach (var step in moves)
        {
            var direction = GetDirection(step);
            var (canMove, movedBoxes) = TryMove(robotPosition, direction, wallPositions, boxPositions);

            if (canMove)
            {
                boxPositions =
                [
                    ..boxPositions.Except(movedBoxes)
                        .Concat(movedBoxes.Select(pos => AddCoords(pos, direction)))
                ];
                robotPosition = AddCoords(robotPosition, direction);
            }
        }

        // Calculate the result based on box positions
        return boxPositions.Sum(pos => 100 * pos.Item1 + pos.Item2);
    }

    private static (int, int) AddCoords((int, int) coord1, (int, int) coord2)
    {
        return (coord1.Item1 + coord2.Item1, coord1.Item2 + coord2.Item2);
    }

    private static (int, int)? FindIntersectingBox((int, int) position, HashSet<(int, int)> boxPositions)
    {
        if (boxPositions.Contains(position)) return position;
        if (boxPositions.Contains(AddCoords(position, (0, -1)))) return AddCoords(position, (0, -1));
        return null;
    }

    // Attempt to move the robot and boxes
    private static (bool, HashSet<(int, int)>) TryMove(
        (int, int) start,
        (int, int) direction,
        HashSet<(int, int)> wallPositions,
        HashSet<(int, int)> boxPositions)
    {
        var target = AddCoords(start, direction);

        // Check for a wall collision
        if (wallPositions.Contains(target))
            return (false, []);

        var targetBox = FindIntersectingBox(target, boxPositions);

        // If no box at target, movement is valid
        if (targetBox == null)
            return (true, []);

        // Handle moving boxes in different directions
        switch (direction)
        {
            case (0, -1): // Left
                var leftMove = TryMove(targetBox.Value, direction, wallPositions, boxPositions);
                return (leftMove.Item1, [..leftMove.Item2, targetBox.Value]);

            case (0, 1): // Right
                var rightMove = TryMove(AddCoords(targetBox.Value, direction), direction, wallPositions, boxPositions);
                return (rightMove.Item1, [..rightMove.Item2, targetBox.Value]);

            default: // Up or Down
                var moveA = TryMove(targetBox.Value, direction, wallPositions, boxPositions);
                var moveB = TryMove(AddCoords(targetBox.Value, (0, 1)), direction, wallPositions, boxPositions);
                return (moveA.Item1 && moveB.Item1,
                    [..moveA.Item2.Concat(moveB.Item2), targetBox.Value]);
        }
    }

    private static (int, int) GetDirection(char instruction)
    {
        return instruction switch
        {
            '^' => (-1, 0),
            '>' => (0, 1),
            'v' => (1, 0),
            '<' => (0, -1),
            _ => (0, 0)
        };
    }
}
