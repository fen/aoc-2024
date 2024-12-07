namespace Aoc.Day07;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        return CalculateTotalCalibration(await inputFile.ReadAllLinesAsync()).ToString();
    }

    static long CalculateTotalCalibration(string[] equations)
    {
        return equations
            .Select(equation => equation.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries))
            .Select(parts => (TestValue: long.Parse(parts[0]), Numbers: parts.Skip(1).Select(long.Parse).ToArray()))
            .Where(tuple => CanMatchTestValue(tuple.TestValue, tuple.Numbers))
            .Sum(tuple => tuple.TestValue);
    }

    static bool CanMatchTestValue(long testValue, long[] numbers)
    {
        var numOperators = numbers.Length - 1;
        char[] operators = ['+', '*'];

        foreach (var combination in GenerateOperatorCombinations(operators, numOperators))
        {
            if (EvaluateExpression(numbers, combination) == testValue)
            {
                return true;
            }
        }

        return false;
    }

    static IEnumerable<string> GenerateOperatorCombinations(char[] operators, int numOperators)
    {
        return GenerateOperatorCombinationsRecursive("", operators, numOperators);

        static IEnumerable<string> GenerateOperatorCombinationsRecursive(string current, char[] operators,
            int numOperators)
        {
            if (numOperators == 0)
            {
                yield return current;
            }
            else
            {
                foreach (var op in operators)
                {
                    foreach (var combination in GenerateOperatorCombinationsRecursive(current + op, operators,
                                 numOperators - 1))
                    {
                        yield return combination;
                    }
                }
            }
        }
    }

    static long EvaluateExpression(long[] numbers, string operators)
    {
        var result = numbers[0];
        for (var i = 0; i < operators.Length; i++)
        {
            if (operators[i] == '+')
            {
                result += numbers[i + 1];
            }
            else if (operators[i] == '*')
            {
                result *= numbers[i + 1];
            }
        }

        return result;
    }
}
