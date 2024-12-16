namespace Aoc.Day16;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var input = await File.ReadAllLinesAsync(inputFile.FullName);

        var maze = input.Select(line => line.ToCharArray()).ToArray();

        var start = FindPosition(maze, 'S');
        var end = FindPosition(maze, 'E');

        // Directions: East, South, West, North (clockwise order)
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        // Priority queue for Dijkstra's Algorithm (min-heap for state cost)
        var pq = new PriorityQueue<State, int>();
        var visited = new HashSet<(int X, int Y, int Dir)>();

        // Add initial state: Start at 'S', facing East, with cost 0
        pq.Enqueue(new State(start.X, start.Y, 0, 0), 0);

        while (pq.TryDequeue(out var curr, out _))
        {
            // Goal state reached
            if ((curr.X, curr.Y) == (end.X, end.Y))
                return curr.Cost.ToString();

            // Mark state as visited
            if (!visited.Add((curr.X, curr.Y, curr.Dir)))
                continue;

            // Explore neighbors (move forward)
            var (dx, dy) = directions[curr.Dir];
            var nx = curr.X + dx;
            var ny = curr.Y + dy;
            if (maze[nx][ny] != '#') // Valid move
            {
                pq.Enqueue(new State(nx, ny, curr.Dir, curr.Cost + 1), curr.Cost + 1);
            }

            // Explore rotations
            for (var rot = -1; rot <= 1; rot += 2) // -1 = counterclockwise, 1 = clockwise
            {
                var newDir = (curr.Dir + rot + 4) % 4; // Ensure direction remains in range [0, 3]
                pq.Enqueue(curr with { Dir = newDir, Cost = curr.Cost + 1000 }, curr.Cost + 1000);
            }
        }

        throw new UnreachableException();
    }

    private static (int X, int Y) FindPosition(char[][] maze, char target)
    {
        for (var i = 0; i < maze.Length; i++)
        for (var j = 0; j < maze[i].Length; j++)
            if (maze[i][j] == target)
                return (i, j);
        throw new InvalidOperationException($"Character {target} not found in maze.");
    }

    sealed record State(int X, int Y, int Dir, int Cost);
}
