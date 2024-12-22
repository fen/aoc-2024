namespace Aoc.Day22;

using PriceChangePattern = (int, int, int, int);

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        // Load all initial secret numbers for the buyers
        var lines = await inputFile.ReadAllLinesAsync();
        var initialSecrets = lines.Select(long.Parse);

        const int steps = 2000; // Generate 2000 prices for each buyer
        const long pruneModulo = 16777216;

        // Dictionary to hold the banana profits of all 4-change patterns
        var bananaProfits = new ConcurrentDictionary<PriceChangePattern, long>();

        // For each buyer (initial secret), calculate banana profits for all patterns
        foreach (var secret in initialSecrets)
        {
            SimulateBuyer(secret, steps, pruneModulo, bananaProfits);
        }

        // Find the 4-change pattern that gives the most bananas across all buyers
        var bestPattern = bananaProfits.OrderByDescending(pair => pair.Value).First();

        return bestPattern.Value.ToString(); // Return the maximum bananas obtained
    }

    private static void SimulateBuyer(
        long secret,
        int steps,
        long pruneModulo,
        ConcurrentDictionary<PriceChangePattern, long> bananaProfits
    )
    {
        // Simulate the buyer's price sequence
        var prices = GeneratePrices(secret, steps, pruneModulo);

        // Track all patterns of 4 consecutive price changes
        var patternsSeen = new HashSet<PriceChangePattern>();

        // Iterate through price change patterns
        for (var i = 0; i <= prices.Count - 5; i++)
        {
            // Extract the 4-change pattern
            var pattern = (
                prices[i + 1] - prices[i],
                prices[i + 2] - prices[i + 1],
                prices[i + 3] - prices[i + 2],
                prices[i + 4] - prices[i + 3]
            );

            // If we've already seen the pattern for this buyer, skip it
            if (!patternsSeen.Add(pattern)) continue;

            // Add the price associated with the pattern's occurrence to the total profit
            bananaProfits.AddOrUpdate(pattern, prices[i + 4], (_, profit) => profit + prices[i + 4]);
        }
    }

    private static List<int> GeneratePrices(long initialSecret, int steps, long pruneModulo)
    {
        var prices = new List<int>();
        var secret = initialSecret;

        for (var i = 0; i <= steps; i++)
        {
            // Add the ones digit of the secret number to prices
            prices.Add((int)(secret % 10));

            // Generate the next secret
            secret = GenerateNextSecret(secret, pruneModulo);
        }

        return prices;
    }

    private static long GenerateNextSecret(long secret, long pruneModulo)
    {
        // Perform the Monkey Exchange Market's pseudorandom number generation
        secret ^= (secret * 64);
        secret %= pruneModulo;
        secret ^= (secret / 32);
        secret %= pruneModulo;
        secret ^= (secret * 2048);
        secret %= pruneModulo;
        return secret;
    }
}
