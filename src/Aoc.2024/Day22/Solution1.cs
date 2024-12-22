namespace Aoc.Day22;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var initialSecrets = lines.Select(long.Parse);

        const int steps = 2000;
        const long pruneModulo = 16777216;

        var sum = initialSecrets
            .Select(secret => ProcessSecret(secret, steps, pruneModulo))
            .Sum();

        return sum.ToString();
    }

    private static long ProcessSecret(long secret, int steps, long pruneModulo)
    {
        for (var i = 0; i < steps; i++)
        {
            secret = GenerateNextSecret(secret, pruneModulo);
        }
        return secret;
    }

    private static long GenerateNextSecret(long secret, long pruneModulo)
    {
        secret ^= (secret * 64);
        secret %= pruneModulo;

        secret ^= (secret / 32);
        secret %= pruneModulo;

        secret ^= (secret * 2048);
        secret %= pruneModulo;

        return secret;
    }
}
