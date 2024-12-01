namespace Aoc;

public interface ISolver
{
    ValueTask<string> SolveAsync(FileInfo inputFile);
}