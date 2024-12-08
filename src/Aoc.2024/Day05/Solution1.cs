namespace Aoc.Day05;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var rules = ParseRules(lines);
        var pages = ParsePages(lines.Skip(rules.Length + 1));

        var count = pages.Sum(pagesToProduce =>
            IsValid(pagesToProduce, rules) ? pagesToProduce[pagesToProduce.Length / 2] : 0);

        return count.ToString();
    }

    private static OrderRule[] ParseRules(string[] lines)
    {
        return lines
            .TakeWhile(l => l.Length > 0)
            .Select(OrderRule.Parse)
            .ToArray();
    }

    private static IEnumerable<int[]> ParsePages(IEnumerable<string> lines)
    {
        return lines
            .Select(l => l.Split(',').Select(int.Parse).ToArray());
    }

    /// Checks if the pages produced are valid for the given rules
    private static bool IsValid(int[] pagesToProduce, OrderRule[] rules)
    {
        for (var pageIndex = 0; pageIndex < pagesToProduce.Length; pageIndex++)
        {
            foreach (var rule in rules)
            {
                if (!rule.Valid(pageIndex, pagesToProduce))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private record struct OrderRule(int X, int Y)
    {
        public static OrderRule Parse(string line)
        {
            var parts = line.Split('|');
            return new OrderRule(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public bool Valid(int i, int[] pages)
        {
            var page = pages[i];

            if (page == X)
            {
                return CheckValidity(Y, pages => pages.Skip(i + 1));
            }

            if (page == Y)
            {
                return CheckValidity(X, pages => pages.Take(i));
            }

            return true;

            bool CheckValidity(int targetPage, Func<IEnumerable<int>, IEnumerable<int>> rangeSelector)
            {
                return pages.All(p => p != targetPage) ||
                       rangeSelector(pages).Any(p => p == targetPage);
            }
        }
    }
}
