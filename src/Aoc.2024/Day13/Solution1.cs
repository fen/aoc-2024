using System.Text.RegularExpressions;

namespace Aoc.Day13;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var input = await inputFile.ReadAllTextAsync();

        var data = ParseInput(input);
        return ClawMachineSolver(data).ToString();
    }

    static List<((int x, int y) buttonA, (int x, int y) buttonB, (int x, int y) prize)> ParseInput(string input)
    {
        // Regex to extract all integers, including negatives
        var numbers = Regex.Matches(input, @"-?\d+")
                           .Select(match => int.Parse(match.Value)) // Parse each match into an int
                           .ToList();

        // Group numbers into chunks of 6 (Button A: x, y; Button B: x, y; Prize: x, y)
        return numbers
            .Chunk(6)
            .Select(chunk => (
                buttonA: (chunk[0], chunk[1]),
                buttonB: (chunk[2], chunk[3]),
                prize: (chunk[4], chunk[5])
            ))
            .ToList();
    }

    private static int ClawMachineSolver(List<((int x, int y) buttonA, (int x, int y) buttonB, (int x, int y) prize)> data)
    {
        var totalCost = 0;

        foreach (var machine in data)
        {
            var (buttonA, buttonB, prize) = machine;
            var cost = SolveClawMachine(buttonA, buttonB, prize);
            if (cost.HasValue)
            {
                totalCost += cost.Value;
            }
        }

        return totalCost;
    }

    // Solve for one claw machine using Cramer's rule
    private static int? SolveClawMachine((int x, int y) buttonA, (int x, int y) buttonB, (int x, int y) prize)
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
        if (detA % detMain != 0 || detB % detMain != 0)
            return null; // Ensure coefficients are integers

        var a = detA / detMain;
        var b = detB / detMain;

        // Both a and b must be non-negative
        if (a < 0 || b < 0)
            return null;

        // Compute total cost
        var cost = (a * 3) + b;
        return cost;
    }

    // Method to calculate determinant of a 2x2 matrix
    static int Determinant(int a, int b, int c, int d)
    {
        return (a * d) - (b * c);
    }
}
