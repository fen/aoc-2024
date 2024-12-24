namespace Aoc.Day13;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var input = await inputFile.ReadAllTextAsync();

        var data = ParseInput(input);
        return ClawMachineSolver(data).ToString();
    }

    static List<((int x, int y) buttonA, (int x, int y) buttonB, (long x, long y) prize)> ParseInput(string input)
    {
        const long prizeOffset = 10000000000000L; // Offset for prize coordinates

        // Regex to extract all integers, including negatives
        var numbers = Regex.Matches(input, @"-?\d+")
                           .Select(match => long.Parse(match.Value)) // Parse into long for large numbers
                           .ToList();

        // Group numbers into chunks of 6 and apply offset to prize coordinates
        return numbers
            .Chunk(6)
            .Select(chunk => (
                buttonA: ((int)chunk[0], (int)chunk[1]),
                buttonB: ((int)chunk[2], (int)chunk[3]),
                prize: (chunk[4] + prizeOffset, chunk[5] + prizeOffset)
            ))
            .ToList();
    }

    static long ClawMachineSolver(List<((int x, int y) buttonA, (int x, int y) buttonB, (long x, long y) prize)> data)
    {
        long totalCost = 0;

        foreach (var machine in data)
        {
            var cost = SolveClawMachine(machine.buttonA, machine.buttonB, machine.prize);
            if (cost.HasValue)
            {
                totalCost += cost.Value;
            }
        }

        return totalCost;
    }

    // Solve for one claw machine using Cramer's rule
    static long? SolveClawMachine((int x, int y) buttonA, (int x, int y) buttonB, (long x, long y) prize)
    {
        var (Ax, Ay) = buttonA;
        var (Bx, By) = buttonB;
        var (Px, Py) = prize;

        // Determinants
        var detMain = Determinant(Ax, Bx, Ay, By);
        if (detMain == 0)
            return null; // No solution, system is inconsistent or lines are parallel

        var detA = Determinant(Px, Bx, Py, By);
        var detB = Determinant(Ax, Px, Ay, Py);

        // Compute a and b
        if (detA % detMain != 0 || detB % detMain != 0) // Ensure no decimals
            return null;

        var a = detA / detMain;
        var b = detB / detMain;

        // Both a and b must be non-negative
        if (a < 0 || b < 0)
            return null;

        // Compute total cost
        var cost = (a * 3) + b;
        return cost;
    }

    // Helper method to compute determinant of a 2x2 matrix
    static long Determinant(long a, long b, long c, long d)
    {
        return (a * d) - (b * c);
    }
}
