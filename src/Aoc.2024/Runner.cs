namespace Aoc;

public static class Runner
{
    public static async Task SolveAsync<TSolver>(DirectoryInfo inputs, bool exampleInput = false)
        where TSolver : ISolver
    {
        var solver = Activator.CreateInstance<TSolver>();
        var inputFile = GetInputFile<TSolver>(inputs, exampleInput);
        Assert(inputFile is not null, $"Unable to find input file for solution {typeof(TSolver).FullName}");
        var result = await solver.SolveAsync(inputFile);
        Console.WriteLine($"{typeof(TSolver).FullName}: {result}");
    }

    private static FileInfo? GetInputFile<TSolver>(DirectoryInfo inputs, bool exampleInput)
    {
        var (_, day, _) = typeof(TSolver).FullName!.Split('.');
        if (exampleInput)
        {
            day = $"{day}-example";
        }

        return inputs.GetFiles(day).FirstOrDefault();
    }
}
