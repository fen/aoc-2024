namespace Aoc.Day17;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var (A, B, C, program) = ParseInput(await inputFile.ReadAllTextAsync());

        var output = new List<int>();

        var ip = 0;

        while (ip < program.Count)
        {
            var opcode = program[ip];
            var operand = program[ip + 1];

            switch (opcode)
            {
                case 0: // adv
                    A /= (int)Math.Pow(2, GetComboValue(operand, A, B, C));
                    break;

                case 1: // bxl
                    B ^= operand; // XOR B with the literal operand
                    break;

                case 2: // bst
                    B = GetComboValue(operand, A, B, C) % 8;
                    break;

                case 3: // jnz
                    if (A != 0)
                    {
                        ip = operand; // Jump to the operand value
                        continue;
                    }
                    break;

                case 4: // bxc
                    B ^= C; // XOR B with C (operand ignored)
                    break;

                case 5: // out
                    output.Add(GetComboValue(operand, A, B, C) % 8);
                    break;

                case 6: // bdv
                    B = A / (int)Math.Pow(2, GetComboValue(operand, A, B, C));
                    break;

                case 7: // cdv
                    C = A / (int)Math.Pow(2, GetComboValue(operand, A, B, C));
                    break;

                default:
                    throw new InvalidOperationException($"Unknown opcode: {opcode}");
            }

            ip += 2; // Move to the next instruction
        }

        return string.Join(",", output);
    }

    private static (int A, int B, int C, List<int> program) ParseInput(string input)
    {
        // Split input into lines
        var lines = input.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

        // Extract registers
        var A = int.Parse(lines[0].Split(":")[1].Trim());
        var B = int.Parse(lines[1].Split(":")[1].Trim());
        var C = int.Parse(lines[2].Split(":")[1].Trim());

        // Extract program
        var program = lines[3].Split(":")[1].Trim()
            .Split(',')
            .Select(int.Parse)
            .ToList();

        return (A, B, C, program);
    }


    private static int GetComboValue(int operand, int A, int B, int C)
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
