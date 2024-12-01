namespace Aoc.Day01;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile) {
        List<int> lefts = [];
        List<int> rights = [];

        foreach (var line in await inputFile.ReadAllLinesAsync()) {
            var (left, right) = line.Split("   ");
            lefts.Add(int.Parse(left));
            rights.Add(int.Parse(right));
        }

        lefts.Sort();
        rights.Sort();

        return lefts
            .Select(l => l * rights.Count(r => r == l))
            .Sum()
            .ToString();
    }
}