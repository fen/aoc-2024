namespace Aoc.Day23;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var graph = await CreateNetworkGraph(inputFile);
        var largestClique = GetLargestClique(graph);
        return CreatePartyPassword(largestClique);
    }

    private static async Task<Dictionary<string, HashSet<string>>> CreateNetworkGraph(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();

        var graph = new Dictionary<string, HashSet<string>>();
        foreach (var line in lines)
        {
            var parts = line.Split('-');
            var a = parts[0];
            var b = parts[1];

            if (!graph.ContainsKey(a)) graph[a] = [];
            if (!graph.ContainsKey(b)) graph[b] = [];

            graph[a].Add(b);
            graph[b].Add(a);
        }

        return graph;
    }

    private static HashSet<string> GetLargestClique(Dictionary<string, HashSet<string>> graph)
    {
        var largestClique = new HashSet<string>();
        BronKerbosch([], [..graph.Keys], [], graph, ref largestClique);
        return largestClique;
    }

    /// <summary>
    /// First I generated all cliques recursively to find the larges but with
    /// this update I remembered that we can use Bron-Kerbosch algorithm.
    ///
    /// The algorithm recursively reduces the size <paramref name="p"/> and
    /// <paramref name="x"/>, ensuring that maximal cliques are built
    /// efficiently.
    ///
    /// At the base case of recursion <paramref name="p"/> and
    /// <paramref name="x"/> are empty, it checks if <see cref="r"/>
    /// is the largest clique so far.
    /// </summary>
    /// <param name="r">is the current clique being built.</param>
    /// <param name="p">contains nodes that can still be added to r.</param>
    /// <param name="x">contains nodes that have already been considered (to avoid duplicate work).</param>
    /// <param name="graph"></param>
    /// <param name="largestClique"></param>
    private static void BronKerbosch(
        HashSet<string> r, HashSet<string> p, HashSet<string> x,
        Dictionary<string, HashSet<string>> graph,
        ref HashSet<string> largestClique)
    {
        if (p.Count == 0 && x.Count == 0)
        {
            if (r.Count > largestClique.Count)
            {
                largestClique = [..r];
            }

            return;
        }

        foreach (var vertex in p.ToList())
        {
            var neighbors = graph[vertex];

            BronKerbosch(
                [..r, vertex],
                [..p.Intersect(neighbors)],
                [..x.Intersect(neighbors)],
                graph,
                ref largestClique);

            p.Remove(vertex);
            x.Add(vertex);
        }
    }

    private static string CreatePartyPassword(HashSet<string> largestClique)
    {
        return string.Join(",", largestClique.OrderBy(static node => node, StringComparer.Ordinal));
    }
}
