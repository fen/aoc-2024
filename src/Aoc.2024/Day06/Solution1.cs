using System.Diagnostics;

namespace Aoc.Day06;

using Direction = (int X, int Y);
using Position = (int X, int Y);

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var map = new Map(await inputFile.ReadCharMapAsync());
        return map.Run().ToString();
    }
}

file sealed record Map(char[][] Input)
{
    private static char[] GuardPositions = ['^', '>', 'v', '<'];
    private static Direction[] GuardDirection = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static char[] GuardTurnPositions = ['>', 'v', '<', '^'];
    private const char Obstacle = '#';

    public int Run()
    {
        var guardPosition = FindGuard();
        var guard = GetAt(guardPosition);
        var direction = GetGuardDirection(guard);

        HashSet<Position> visitedPositions = [guardPosition];
        while (MoveGuard(ref guardPosition, ref direction))
        {
            visitedPositions.Add(guardPosition);
        }

        return visitedPositions.Count;
    }

    private Position FindGuard()
    {
        for (var y = 0; y < Input.Length; y++)
        {
            var line = Input[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (GuardPositions.Contains(line[x]))
                {
                    return (x, y);
                }
            }
        }

        Assert(false, "Could not find guard position");
        return (-1, -1);
    }

    private char GetAt(Position position)
    {
        return Input[position.Y][position.X];
    }

    private static Direction GetGuardDirection(char guard)
    {
        var index = Array.IndexOf(GuardPositions, guard);
        return GuardDirection[index];
    }

    private bool MoveGuard(ref Position currentPosition, ref Direction direction)
    {
        Position nextPosition = (currentPosition.X + direction.X, currentPosition.Y + direction.Y);

        if (IsOutOfBounds(nextPosition))
        {
            return false;
        }

        if (GetAt(nextPosition) is Obstacle)
        {
            var turnedGuard = TurnGuard90Degrees(currentPosition);
            Input[currentPosition.Y][currentPosition.X] = turnedGuard;
            direction = GetGuardDirection(turnedGuard);
            nextPosition = (currentPosition.X + direction.X, currentPosition.Y + direction.Y);

            if (IsOutOfBounds(nextPosition))
            {
                return false;
            }
        }

        Input[nextPosition.Y][nextPosition.X] = Input[currentPosition.Y][currentPosition.X];
        Input[currentPosition.Y][currentPosition.X] = '.';

        currentPosition = nextPosition;
        return true;
    }

    private bool IsOutOfBounds(Position nextPosition)
    {
        return nextPosition.X < 0 || nextPosition.X >= Input[0].Length ||
               nextPosition.Y < 0 || nextPosition.Y >= Input.Length;
    }

    private char TurnGuard90Degrees(Position currentPosition)
    {
        var index = Array.IndexOf(GuardPositions, GetAt(currentPosition));
        return GuardTurnPositions[index];
    }
}
