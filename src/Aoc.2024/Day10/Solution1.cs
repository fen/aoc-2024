namespace Aoc.Day10;

using Keypad = Dictionary<char, (int X, int Y)>;

public class Solution1 : ISolver
{
    // Define the keypads
    private static readonly Keypad NumericButtons = new()
    {
        ['7'] = (0, 0), ['8'] = (1, 0), ['9'] = (2, 0),
        ['4'] = (0, 1), ['5'] = (1, 1), ['6'] = (2, 1),
        ['1'] = (0, 2), ['2'] = (1, 2), ['3'] = (2, 2),
        ['X'] = (0, 3), ['0'] = (1, 3), ['A'] = (2, 3)
    };

    private static readonly Keypad DirectionalButtons = new()
    {
        ['X'] = (0, 0), ['^'] = (1, 0), ['A'] = (2, 0),
        ['<'] = (0, 1), ['v'] = (1, 1), ['>'] = (2, 1)
    };

    // Keypads for different historians
    private static readonly Keypad[] KeypadsForFirstHistorian =
        [NumericButtons, ..Enumerable.Repeat(DirectionalButtons, 2).ToArray()];

    private static readonly Keypad[] KeypadsForSecondHistorian =
        [NumericButtons, ..Enumerable.Repeat(DirectionalButtons, 25).ToArray()];

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var codes = await inputFile.ReadAllLinesAsync();

        long result1 = 0, result2 = 0;

        foreach (var code in codes)
        {
            var numericPart = long.Parse(code.Where(char.IsDigit).ToArray());

            // Compute results for both historians
            result1 += FindShortestSequenceLength(code, KeypadsForFirstHistorian) * numericPart;
            result2 += FindShortestSequenceLength(code, KeypadsForSecondHistorian) * numericPart;
        }

        Console.WriteLine($"Results: Part 1 = {result1}, Part 2 = {result2}");
        return $"{result1}, {result2}";
    }

    private static long FindShortestSequenceLength(string sequence, Keypad[] keypads)
    {
        // Use an empty cache wrapper for the recursive implementation
        return FindShortestSequenceLength(sequence, keypads, new Dictionary<(string, int), long>());
    }

    private static long FindShortestSequenceLength(
        string sequence,
        Keypad[] keypads,
        Dictionary<(string, int), long> cache)
    {
        // Look up the cache first
        if (cache.TryGetValue((sequence, keypads.Length), out var length))
            return length;

        // Initialize starting parameters
        var position = keypads[0]['A'];
        var forbiddenPosition = keypads[0]['X'];
        length = 0;

        foreach (var button in sequence)
        {
            var shortestLength = long.MaxValue;
            var targetPosition = keypads[0][button];

            // Compute all move sequences to the target
            foreach (var moveSequence in GetMoveSequences(position, targetPosition, forbiddenPosition))
            {
                if (keypads.Length == 1)
                {
                    // Base case for recursion: Single keypad remaining
                    shortestLength = moveSequence.Length;
                    break;
                }

                // Recursive call for the rest of the keypads
                shortestLength = Math.Min(
                    shortestLength,
                    FindShortestSequenceLength(moveSequence, keypads[1..], cache)
                );
            }

            // Aggregate total path length
            length += shortestLength;
            position = targetPosition;
        }

        // Cache and return the computed length
        return cache[(sequence, keypads.Length)] = length;
    }

    private static List<string> GetMoveSequences((int X, int Y) position, (int X, int Y) targetPosition, (int X, int Y) forbiddenPosition)
    {
        var result = new List<string>();

        // Move horizontally first, then vertically
        var nextHorizontalPosition = (targetPosition.X, position.Y);
        var nextVerticalPosition = (position.X, targetPosition.Y);

        // Horizontal move logic
        if (position != nextHorizontalPosition && nextHorizontalPosition != forbiddenPosition)
        {
            var prefix = new string(position.X < targetPosition.X ? '>' : '<', Math.Abs(targetPosition.X - position.X));
            var suffixes = GetMoveSequences(nextHorizontalPosition, targetPosition, forbiddenPosition);
            result.AddRange(suffixes.Select(s => prefix + s));
        }

        // Vertical move logic
        if (position != nextVerticalPosition && nextVerticalPosition != forbiddenPosition)
        {
            var prefix = new string(position.Y < targetPosition.Y ? 'v' : '^', Math.Abs(targetPosition.Y - position.Y));
            var suffixes = GetMoveSequences(nextVerticalPosition, targetPosition, forbiddenPosition);
            result.AddRange(suffixes.Select(s => prefix + s));
        }

        // Ensure fallback if no valid moves exist
        return result.Count > 0 ? result : new List<string> { "A" };
    }
}
