namespace Aoc.Day05;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var rules = lines
            .TakeWhile(l => l.Length > 0)
            .Select(OrderRule.Parse)
            .ToArray();

        var pages = lines.Skip(rules.Length + 1)
            .Select(l => l.Split(',').Select(int.Parse).ToList());

        var count = 0;
        foreach (var pagesToProduce in pages)
        {
            var valid = true;
            for (var i = 0; i < pagesToProduce.Count; i++)
            {
                if (rules.Any(rule => !rule.Valid(i, pagesToProduce)))
                {
                    valid = false;
                }

                if (!valid)
                {
                    break;
                }
            }

            if (!valid)
            {
                pagesToProduce.Sort(rules);
                count += pagesToProduce[pagesToProduce.Count / 2];
            }
        }

        return count.ToString();
    }
}

file static class Helper
{
    public static void Sort(this List<int> pages, OrderRule[] rules)
    {
        bool again;
        do
        {
            again = false;

            foreach (var rule in rules)
            {
                if (rule.Fix(pages))
                {
                    again = true;
                }
            }
        } while (again);
    }
}

file record struct OrderRule(int X, int Y)
{
    public static OrderRule Parse(string line)
    {
        var parts = line.Split('|');
        return new OrderRule(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public bool Valid(int i, List<int> pages)
    {
        var page = pages[i];
        if (page == X)
        {
            var y = Y;
            if (pages.All(p => p != y))
            {
                return true;
            }

            return pages.Skip(i + 1).Any(p => p == y);
        }

        if (page == Y)
        {
            var x = X;
            if (pages.All(p => p != x))
            {
                return true;
            }

            return pages.Take(i).Any(p => p == x);
        }

        return true;
    }

    public bool Fix(List<int> pages)
    {
        var y = -1;
        var x = -1;
        for (var i = 0; i < pages.Count; i++)
        {
            var page = pages[i];
            if (page == X)
            {
                x = i;
                break;
            }

            if (page == Y)
            {
                y = i;
            }
        }

        if (x == -1 || y == -1)
        {
            return false;
        }

        var tmp = pages[x];
        pages.RemoveAt(x);
        pages.Insert(y, tmp);

        return true;
    }
}
