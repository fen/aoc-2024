using System.Diagnostics;
using System.Numerics;

namespace Aoc.Day06;

using Direction = (int X, int Y);
using Position = (int X, int Y);
using GuardState = ((int X, int Y) Position, (int X, int Y) Direction);

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();


        char[][] charArray = new char[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            charArray[i] = lines[i].ToCharArray();
        }

        var map = new Map(charArray);

        return map.Run().ToString();
    }
}

sealed file record Map(char[][] Input)
{
    private static readonly char[] GuardPositions = ['^', '>', 'v', '<'];
    private static readonly Direction[] GuardDirection = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static readonly char[] GuardTurnPositions = ['>', 'v', '<', '^'];
    private const char Obstacle = '#';
    private const char Empty = '.';

    public int Run()
    {
        var guardStartPosition = FindGuard();
        var guard = GetAt(guardStartPosition);
        var guardStartDirection = GetGuardDirection(guard);

        var guardPosition = guardStartPosition;
        var direction = guardStartDirection;

        HashSet<Position> visitedPositions = [];
        while (MoveGuard(ref guardPosition, ref direction))
        {
            if (guardPosition == guardStartPosition)
            {
                continue;
            }

            visitedPositions.Add(guardPosition);
        }

        int count = 0;
        foreach (var p in visitedPositions.Where(p => GetAt(p) == Empty))
        {
            MoveGuard(guardPosition, guardStartPosition, guard);
            guardPosition = guardStartPosition;
            direction = guardStartDirection;

            PutObstacle(p);

            if (IsLoop(ref guardPosition, ref direction))
            {
                count += 1;
            }

            RemoveObstacle(p);
        }

        return count;
    }

    private bool IsLoop(ref Position guardPosition, ref Direction direction)
    {
        HashSet<GuardState> visitedStates = [];
        while (MoveGuard(ref guardPosition, ref direction))
        {
            if (!visitedStates.Add((guardPosition, direction)))
            {
                return true;
            }
        }

        return false;
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
        }
        else
        {
            Input[nextPosition.Y][nextPosition.X] = Input[currentPosition.Y][currentPosition.X];
            Input[currentPosition.Y][currentPosition.X] = '.';
            currentPosition = nextPosition;
        }

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

    private void PutObstacle(Position position)
    {
        Input[position.Y][position.X] = Obstacle;
    }

    private void RemoveObstacle(Position position)
    {
        Input[position.Y][position.X] = '.';
    }

    private void MoveGuard(Position from, Position to, char guard)
    {
        Input[to.Y][to.X] = guard;
        Input[from.Y][from.X] = '.';
    }
}
