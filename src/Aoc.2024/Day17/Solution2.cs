using System.Runtime.InteropServices;

namespace Aoc.Day17;

public class Solution2 : ISolver
{
    private readonly List<int> _output = [];

    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var (A, B, C, program) = ParseInput(await inputFile.ReadAllTextAsync());

        return FindValue(program, A, B, C).ToString();
    }

    private long FindValue(List<int> program, long A, long B, long C)
    {
        var result = 1L;

        var programSpan = CollectionsMarshal.AsSpan(program);

        for (var i = 0; i < programSpan.Length; ++i)
        {
            var outputMatched = false;

            for (result = (result - 1L) * 8L; !outputMatched; ++result)
            {
                A = result;
                var output = Run(program, A, B, C);

                var outputSpan = CollectionsMarshal.AsSpan(output);
                if (outputSpan.Length <= programSpan.Length)
                {
                    var programTail = programSpan[^outputSpan.Length..];
                    outputMatched = programTail.SequenceEqual(outputSpan);
                }
            }
        }

        return result - 1L;
    }

    private List<int> Run(List<int> program, long A, long B, long C)
    {
        _output.Clear();
        var output = _output;

        var ip = 0;

        while (ip < program.Count)
        {
            var opcode = program[ip];
            var operand = program[ip + 1];

            var comboValue = GetComboValue(operand, A, B, C);

            switch (opcode)
            {
                case 0: // adv
                    A >>= (int)comboValue;
                    break;

                case 1: // bxl
                    B ^= operand; // XOR B with the literal operand
                    break;

                case 2: // bst
                    B = comboValue & 0b111L;
                    break;

                case 3 when A != 0L: // jnz
                    ip = operand;
                    continue;

                case 4: // bxc
                    B ^= C;
                    break;

                case 5: // out
                    output.Add((int)comboValue & 0b111);
                    break;

                case 6: // bdv
                    B = A >> (int)comboValue;
                    break;

                case 7: // cdv
                    C = A >> (int)comboValue;
                    break;
            }

            ip += 2;
        }

        return output;
    }

    private static (long A, long B, long C, List<int> program) ParseInput(string input)
    {
        var lines = input.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

        var A = long.Parse(lines[0].Split(":")[1].Trim());
        var B = long.Parse(lines[1].Split(":")[1].Trim());
        var C = long.Parse(lines[2].Split(":")[1].Trim());

        var program = lines[3].Split(":")[1].Trim()
            .Split(',')
            .Select(int.Parse)
            .ToList();

        return (A, B, C, program);
    }

    private static long GetComboValue(int operand, long A, long B, long C)
    {
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => A,
            5 => B,
            6 => C,
            _ => throw new InvalidOperationException("Invalid combo operand")
        };
    }
}
