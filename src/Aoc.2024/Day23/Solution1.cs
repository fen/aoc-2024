namespace Aoc.Day23;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);

        var graph = ParseInput(lines);
        var triangles = FindTriangles(graph);
        var filteredTriangles = FilterTriangles(triangles);

        return filteredTriangles.Count.ToString();
    }

    private static Dictionary<string, HashSet<string>> ParseInput(string[] lines)
    {
        var graph = new Dictionary<string, HashSet<string>>();

        foreach (var line in lines)
        {
            var parts = line.Split('-');
            var a = parts[0];
            var b = parts[1];

            if (!graph.ContainsKey(a)) graph[a] = new HashSet<string>();
            if (!graph.ContainsKey(b)) graph[b] = new HashSet<string>();

            graph[a].Add(b);
            graph[b].Add(a);
        }

        return graph;
    }

    private static List<HashSet<string>> FindTriangles(Dictionary<string, HashSet<string>> graph)
    {
        var triangles = new List<HashSet<string>>();

        var nodes = graph.Keys.ToList();

        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                for (int k = j + 1; k < nodes.Count; k++)
                {
                    var a = nodes[i];
                    var b = nodes[j];
                    var c = nodes[k];

                    if (graph[a].Contains(b) && graph[a].Contains(c) && graph[b].Contains(c))
                    {
                        triangles.Add([a, b, c]);
                    }
                }
            }
        }

        return triangles;
    }

    private static List<HashSet<string>> FilterTriangles(List<HashSet<string>> triangles)
    {
        return triangles
            .Where(triangle => triangle.Any(node => node.StartsWith("t", StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
}
