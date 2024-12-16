using System.Text.RegularExpressions;

namespace Aoc;

public static class Runner
{
    public static async Task SolveAsync<TSolver>(DirectoryInfo inputs, bool exampleInput = false)
        where TSolver : ISolver
    {
        var sw = Stopwatch.StartNew();
        var solver = Activator.CreateInstance<TSolver>();
        var inputFile = GetInputFile<TSolver>(inputs, exampleInput);
        Assert(inputFile is not null, $"Unable to find input file for solution {typeof(TSolver).FullName}");
        var result = await solver.SolveAsync(inputFile);
        Console.WriteLine($"{typeof(TSolver).FullName}: {result} ({sw.ElapsedMilliseconds}ms)");
    }

    private static FileInfo? GetInputFile<TSolver>(DirectoryInfo inputs, bool exampleInput)
    {
        var (_, day, solution) = typeof(TSolver).FullName!.Split('.');
        var solutionNumber = GetSolutionNumber(solution);

        if (exampleInput)
        {
            day = $"{day}-example{solutionNumber}";
        }

        return inputs.GetFiles(day).FirstOrDefault();
    }

    private static int GetSolutionNumber(string solution)
    {
        var match = Regex.Match(solution, @"\d+");
        return int.Parse(match.Value);
    }
}
