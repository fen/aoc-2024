namespace Aoc.Day05;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var rules = lines
            .TakeWhile(l => l.Length > 0)
            .Select(OrderRule.Parse)
            .ToArray();


        var pages = lines.Skip(rules.Length + 1)
            .Select(l => l.Split(',').Select(int.Parse).ToArray());


        var count = 0;
        foreach (var pagesToProduce in pages)
        {
            var valid = true;
            for (var i = 0; i < pagesToProduce.Length; i++)
            {
                foreach (var rule in rules)
                {
                    if (!rule.Valid(i, pagesToProduce))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    break;
                }
            }

            if (valid)
            {
                count += pagesToProduce[pagesToProduce.Length / 2];
            }
        }


        return count.ToString();
    }
}

file record struct OrderRule(int X, int Y)
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
}
