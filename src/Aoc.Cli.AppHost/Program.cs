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
//
// await SolveAsync<Aoc.Day03.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day03.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day04.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day04.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day05.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day05.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day06.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day06.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day07.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day07.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day08.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day08.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day09.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day09.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day10.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day10.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day11.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day11.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day12.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day12.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day13.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day13.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day14.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day14.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day15.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day15.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day16.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day16.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day17.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day17.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day18.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day18.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day19.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day19.Solution2>(inputs, exampleInput);
//
// await SolveAsync<Aoc.Day20.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day20.Solution2>(inputs, exampleInput);

// await SolveAsync<Aoc.Day21.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day21.Solution2>(inputs, exampleInput);

// await SolveAsync<Aoc.Day22.Solution1>(inputs, exampleInput);
// await SolveAsync<Aoc.Day22.Solution2>(inputs, exampleInput);

await SolveAsync<Aoc.Day23.Solution1>(inputs, exampleInput);
await SolveAsync<Aoc.Day23.Solution2>(inputs, exampleInput);
