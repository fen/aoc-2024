namespace Aoc.Day01;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile) {
        List<int> lefts = [];
        List<int> rights = [];

        foreach (var line in await inputFile.ReadAllLinesAsync()) {
            var (left, right) = line.Split("   ");
            lefts.Add(int.Parse(left, CultureInfo.InvariantCulture));
            rights.Add(int.Parse(right, CultureInfo.InvariantCulture));
        }

        lefts.Sort();
        rights.Sort();

        return lefts
            .Zip(rights, (l, r) => Math.Abs(l - r))
            .Sum()
            .ToString(CultureInfo.InvariantCulture);
    }
}
