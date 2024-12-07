namespace Aoc.Day07;

public class Solution2 : ISolver
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
        string[] operators = ["+", "*", "||"];

        foreach (var combination in GenerateOperatorCombinations(operators, numOperators))
        {
            if (EvaluateExpression(numbers, combination) == testValue)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<string> GenerateOperatorCombinations(string[] operators, int numOperators)
    {
        return GenerateOperatorCombinationsRecursive("", operators, numOperators);

        static IEnumerable<string> GenerateOperatorCombinationsRecursive(string current, string[] operators,
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
                    foreach (var combinations in GenerateOperatorCombinationsRecursive(current + op, operators,
                                 numOperators - 1))
                    {
                        yield return combinations;
                    }
                }
            }
        }
    }

    static long EvaluateExpression(long[] numbers, string operators)
    {
        var result = numbers[0];
        for (int i = 1, j = 0; j < operators.Length; i++, j++)
        {
            result = operators[j] switch
            {
                '+' => result + numbers[i],
                '*' => result * numbers[i],
                '|' when j + 1 < operators.Length && operators[j + 1] == '|' =>
                    long.Parse($"{result}{numbers[i]}"),
                _ => result
            };

            if (operators[j] == '|' && j + 1 < operators.Length && operators[j + 1] == '|')
            {
                j++; // Skip additional '|'
            }
        }

        return result;
    }
}
