using Aoc;
using Aoc.Internal;

DirectoryInfo inputs = new("./inputs");
bool exampleInput = args switch {
    ["--example"] => true,
    _ => false
};

await SolveAsync<Aoc.Day01.Solution1>(inputs, exampleInput);
await SolveAsync<Aoc.Day01.Solution2>(inputs, exampleInput);

static async Task SolveAsync<TSolver>(DirectoryInfo inputs, bool exampleInput = false) where TSolver : ISolver {
    var solver = Activator.CreateInstance<TSolver>();
    var inputFile = GetInputFile<TSolver>(inputs, exampleInput);
    var result = await solver.SolveAsync(inputFile);
    Console.WriteLine($"{typeof(TSolver).FullName}: {result}");
}

static FileInfo GetInputFile<TSolver>(DirectoryInfo inputs, bool exampleInput) {
    var (_, day, _) = typeof(TSolver).FullName!.Split('.');
    if (exampleInput) {
        day = $"{day}-example";
    }

    return inputs.GetFiles(day).First();
}