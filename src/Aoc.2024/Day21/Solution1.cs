namespace Aoc.Day21;

using Keypad = Dictionary<char, (int X, int Y)>;

public class Solution1 : ISolver
{
    private static readonly Keypad NumericButtons = GenerateNumericKeypad();
    private static readonly Keypad DirectionalButtons = GenerateDirectionalKeypad();

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var codes = await inputFile.ReadAllLinesAsync();

        Keypad[] keypadsForFirstHistorian =
            [NumericButtons, ..Enumerable.Repeat(DirectionalButtons, 2).ToArray()];

        long result = 0;
        foreach (var code in codes)
        {
            var numericPart = long.Parse(code.Where(char.IsDigit).ToArray());
            result += CalculateShortestKeypadPath(code, keypadsForFirstHistorian, []) * numericPart;
        }

        return result.ToString();
    }

    private static long CalculateShortestKeypadPath(
        string sequence,
        Keypad[] keypads,
        Dictionary<(string, int), long> cache)
    {
        if (cache.TryGetValue((sequence, keypads.Length), out var cachedLength))
            return cachedLength;

        var position = keypads[0]['A'];
        var forbiddenPosition = keypads[0]['X'];
        var totalLength = 0L;

        foreach (var button in sequence)
        {
            var targetPosition = keypads[0][button];

            var shortestLength = GenerateKeypadMoveSequence(position, targetPosition, forbiddenPosition)
                .Min(moveSequence => keypads.Length == 1
                    ? moveSequence.Length // Base case: Single keypad
                    : CalculateShortestKeypadPath(moveSequence, keypads[1..], cache));

            totalLength += shortestLength;
            position = targetPosition;
        }

        return cache[(sequence, keypads.Length)] = totalLength;
    }

    private static List<string> GenerateKeypadMoveSequence((int X, int Y) position, (int X, int Y) target,
        (int X, int Y) forbidden)
    {
        // Base case: If already at the target, press the button
        if (position == target)
            return ["A"];

        var moves = new List<string>();
        var directionX = position.X < target.X ? '>' : '<';
        var directionY = position.Y < target.Y ? 'v' : '^';

        // Horizontal moves first (if valid)
        if (position.X != target.X)
        {
            var horizontalStep = new string(directionX, Math.Abs(position.X - target.X));
            moves.AddRange(CreateMovementPath(horizontalStep, (target.X, position.Y), target, forbidden));
        }

        // Vertical moves next (if valid)
        if (position.Y != target.Y)
        {
            var verticalStep = new string(directionY, Math.Abs(position.Y - target.Y));
            moves.AddRange(CreateMovementPath(verticalStep, (position.X, target.Y), target, forbidden));
        }

        return moves;
    }

    private static IEnumerable<string> CreateMovementPath(
        string prefix,
        (int X, int Y) nextPosition,
        (int X, int Y) target,
        (int X, int Y) forbidden)
    {
        // Check if next move reaches the forbidden zone
        if (nextPosition == forbidden)
            return [];

        // Add recursive sequences from the new position
        var remainingSteps = GenerateKeypadMoveSequence(nextPosition, target, forbidden);
        return remainingSteps.Select(step => prefix + step);
    }

    private static Keypad GenerateNumericKeypad() =>
        ((string[]) ["789", "456", "123", "X0A"])
            .SelectMany((row, y) => row.Select((key, x) => new { key, position = (x, y) }))
            .ToDictionary(item => item.key, item => item.position);

    private static Keypad GenerateDirectionalKeypad() =>
        ((string[])["X^A", "<v>"])
        .SelectMany((row, y) => row.Select((key, x) => new { key, position = (x, y) }))
        .ToDictionary(item => item.key, item => item.position);
}
