DirectoryInfo inputs = new("./inputs");
bool exampleInput = args switch
{
    ["--example"] => true,
    _ => false
};

// await SolveAsync<Aoc.Day01.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day01.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day02.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day02.Solution2>(inputs, exampleInput);

await SolveAsync<Aoc.Day03.Solution1>(inputs, exampleInput);
await SolveAsync<Aoc.Day03.Solution2>(inputs, exampleInput);
