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
            .AsParallel()
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
            if (EvaluateExpression(numbers, combination, testValue))
            {
                return true;
            }
        }

        return false;
    }

    static IEnumerable<string> GenerateOperatorCombinations(string[] operators, int numOperators)
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

    static bool EvaluateExpression(long[] numbers, string operators, long testValue)
    {
        var result = numbers[0];
        for (int i = 1, j = 0; j < operators.Length; i++, j++)
        {
            result = operators[j] switch
            {
                '+' => result + numbers[i],
                '*' => result * numbers[i],
                '|' => ConcatenateDigits(result, numbers[i]),
                _ => result
            };

            if (operators[j] == '|' && j + 1 < operators.Length && operators[j + 1] == '|')
                j++; // Skip additional '|'

            if (result > testValue)
                return false;
        }

        return result == testValue;

        static long ConcatenateDigits(long a, long b)
        {
            long scale = 1;
            while (scale <= b)
                scale *= 10;
            return a * scale + b;
        }
    }
}
