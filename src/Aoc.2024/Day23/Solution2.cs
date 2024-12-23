namespace Aoc.Day23;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await File.ReadAllLinesAsync(inputFile.FullName);

        var graph = CreateNetworkGraph(lines);

        // Find all cliques
        var allCliques = GetAllCliques(graph);

        // Find the largest clique
        var largestClique = GetLargestClique(allCliques);

        // Generate the password from the largest clique
        var password = CreatePartyPassword(largestClique);

        return password;
    }

    private static Dictionary<string, HashSet<string>> CreateNetworkGraph(string[] lines)
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

    private static List<HashSet<string>> GetAllCliques(Dictionary<string, HashSet<string>> graph)
    {
        var allCliques = new List<HashSet<string>>();
        var currentClique = new HashSet<string>();
        var nodes = graph.Keys.ToList();

        FindCliquesRecursive(graph, nodes, currentClique, allCliques, 0);
        return allCliques;
    }

    private static void FindCliquesRecursive(
        Dictionary<string, HashSet<string>> graph,
        List<string> nodes,
        HashSet<string> currentClique,
        List<HashSet<string>> allCliques,
        int startIndex)
    {
        // Add the current clique to the list of discovered cliques
        allCliques.Add([..currentClique]);

        // Try adding additional nodes to the current clique
        for (int i = startIndex; i < nodes.Count; i++)
        {
            var node = nodes[i];

            // Check if the node is connected to all nodes in the current clique
            if (currentClique.All(other => graph[other].Contains(node)))
            {
                currentClique.Add(node);
                FindCliquesRecursive(graph, nodes, currentClique, allCliques, i + 1);
                currentClique.Remove(node); // Backtrack
            }
        }
    }

    private static HashSet<string> GetLargestClique(List<HashSet<string>> cliques)
    {
        return cliques.OrderByDescending(clique => clique.Count).First();
    }

    private static string CreatePartyPassword(HashSet<string> largestClique)
    {
        return string.Join(",", largestClique.OrderBy(node => node));
    }
}
