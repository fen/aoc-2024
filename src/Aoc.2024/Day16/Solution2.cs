namespace Aoc.Day16;

public class Solution2 : ISolver
{
    private readonly (int DX, int DY) North = (0, -1);
    private readonly (int DX, int DY) East = (1, 0);
    private readonly (int DX, int DY) South = (0, 1);
    private readonly (int DX, int DY) West = (-1, 0);

    private Dictionary<(int DX, int DY), (int DX, int DY)> Left;
    private Dictionary<(int DX, int DY), (int DX, int DY)> Right;

    public Solution2()
    {
        // Define turns: left and right mappings
        Left = new Dictionary<(int DX, int DY), (int DX, int DY)>
        {
            [North] = West, [East] = North, [South] = East, [West] = South
        };
        Right = new Dictionary<(int DX, int DY), (int DX, int DY)>
        {
            [North] = East, [East] = South, [South] = West, [West] = North
        };
    }

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        const char WalkableTile = '.';
        const char StartTile = 'S';
        const char EndTile = 'E';

        var input = await File.ReadAllLinesAsync(inputFile.FullName);
        var maze = input.Select(line => line.ToCharArray()).ToArray();

        var start = FindPosition(maze, StartTile);
        var end = FindPosition(maze, EndTile);

        maze[start.Y][start.X] = WalkableTile;
        maze[end.Y][end.X] = WalkableTile;

        var unvisited =
            new Dictionary<((int X, int Y) Pos, (int DX, int DY) Heading), (int Cost, HashSet<(int, int)> Path)>();
        var nodesByCost = new SortedDictionary<int, List<((int X, int Y), (int DX, int DY))>>();

        InitializeUnvisited(start, unvisited, nodesByCost);

        while (nodesByCost.Any())
        {
            var (currentNode, cost, path) = GetCurrentNodeWithLowestCost(nodesByCost, unvisited);

            if (currentNode.Pos == end)
                return path.Count.ToString();

            ExploreNeighbors(currentNode, cost, path, maze, unvisited, nodesByCost);
        }

        throw new UnreachableException();
    }

    private void InitializeUnvisited((int X, int Y) startPosition,
        Dictionary<((int X, int Y), (int DX, int DY)), (int Cost, HashSet<(int, int)> Path)> unvisited,
        SortedDictionary<int, List<((int X, int Y), (int DX, int DY))>> nodesByCost)
    {
        foreach (var heading in new[] { North, East, South, West })
        {
            var initialState = (startPosition, heading);
            unvisited[initialState] = (0, new HashSet<(int, int)> { startPosition });

            if (!nodesByCost.ContainsKey(0))
                nodesByCost[0] = new List<((int X, int Y), (int DX, int DY))>();

            nodesByCost[0].Add(initialState);
        }
    }

    private static (((int X, int Y) Pos, (int DX, int DY) Heading), int, HashSet<(int, int)>)
        GetCurrentNodeWithLowestCost(
            SortedDictionary<int, List<((int X, int Y), (int DX, int DY))>> nodesByCostCollection,
            Dictionary<((int X, int Y), (int DX, int DY)), (int Cost, HashSet<(int, int)> Path)> unvisitedNodes)
    {
        var minCost = nodesByCostCollection.First();
        var currentNode = minCost.Value[0];

        minCost.Value.Remove(currentNode);
        if (!minCost.Value.Any())
            nodesByCostCollection.Remove(minCost.Key);

        var (pos, heading) = currentNode;
        var (cost, path) = unvisitedNodes[currentNode];
        unvisitedNodes.Remove(currentNode);

        return (currentNode, cost, path);
    }

    private void ExploreNeighbors(((int X, int Y) Pos, (int DX, int DY) Heading) currentNode,
        int currentCost,
        HashSet<(int, int)> currentPath,
        char[][] mazeData,
        Dictionary<((int X, int Y), (int DX, int DY)), (int Cost, HashSet<(int, int)> Path)> unvisitedNodes,
        SortedDictionary<int, List<((int X, int Y), (int DX, int DY))>> costNodes)
    {
        UpdateNodeCost((currentNode.Pos, Left[currentNode.Heading]), currentCost + 1000, currentPath, unvisitedNodes,
            costNodes);
        UpdateNodeCost((currentNode.Pos, Right[currentNode.Heading]), currentCost + 1000, currentPath, unvisitedNodes,
            costNodes);

        var forwardPosition = (X: currentNode.Pos.X + currentNode.Heading.DX,
            Y: currentNode.Pos.Y + currentNode.Heading.DY);

        if (mazeData[forwardPosition.Y][forwardPosition.X] == '.')
        {
            var newPath = new HashSet<(int, int)>(currentPath) { forwardPosition };
            UpdateNodeCost((forwardPosition, currentNode.Heading), currentCost + 1, newPath, unvisitedNodes, costNodes);
        }
    }

    private static void UpdateNodeCost(((int X, int Y) Pos, (int DX, int DY) Heading) node,
        int cost,
        HashSet<(int, int)> path,
        Dictionary<((int X, int Y), (int DX, int DY)), (int Cost, HashSet<(int, int)> Path)> unvisitedNodes,
        SortedDictionary<int, List<((int X, int Y), (int DX, int DY))>> costSortedNodes)
    {
        if (unvisitedNodes.TryGetValue(node, out var existing) && cost >= existing.Cost)
        {
            if (cost == existing.Cost)
                foreach (var p in path)
                    existing.Path.Add(p);
            return;
        }

        if (unvisitedNodes.TryGetValue(node, out var value))
        {
            var oldCost = value.Cost;
            costSortedNodes[oldCost]?.Remove(node);
            if (!costSortedNodes[oldCost].Any())
                costSortedNodes.Remove(oldCost);
        }

        unvisitedNodes[node] = (cost, new HashSet<(int, int)>(path));
        if (!costSortedNodes.ContainsKey(cost))
            costSortedNodes[cost] = new List<((int, int), (int, int))>();

        costSortedNodes[cost].Add(node);
    }

    private static (int X, int Y) FindPosition(char[][] maze, char target)
    {
        for (int y = 0; y < maze.Length; y++)
        {
            for (int x = 0; x < maze[y].Length; x++)
            {
                if (maze[y][x] == target) return (x, y);
            }
        }

        throw new InvalidOperationException($"Character {target} not found in the maze");
    }
}
