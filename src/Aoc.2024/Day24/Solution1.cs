namespace Aoc.Day24;

public class Solution1 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        var (wireStates, gates) = ParseWireDefinitions(lines);
        SimulateLogicGates(wireStates, gates);
        return CalculateOutputValue(wireStates).ToString();
    }

    private static (Dictionary<string, int> wireStates, List<LogicGate> gates) ParseWireDefinitions(string[] lines)
    {
        var wireStates = new Dictionary<string, int>();
        var gates = new List<LogicGate>();

        var initialWireRegex = new Regex(@"^(\w+):\s*(\d+)$");
        var gateDefinitionRegex = new Regex(@"^(\w+)\s+(AND|OR|XOR)\s+(\w+)\s+->\s*(\w+)$");

        foreach (var line in lines)
        {
            var initialMatch = initialWireRegex.Match(line);
            if (initialMatch.Success)
            {
                wireStates[initialMatch.Groups[1].Value] = int.Parse(initialMatch.Groups[2].Value);
                continue;
            }

            var gateMatch = gateDefinitionRegex.Match(line);
            if (gateMatch.Success)
            {
                gates.Add(new LogicGate(
                    gateMatch.Groups[2].Value,
                    gateMatch.Groups[1].Value,
                    gateMatch.Groups[3].Value,
                    gateMatch.Groups[4].Value
                ));
            }
        }

        return (wireStates, gates);
    }

    private static void SimulateLogicGates(Dictionary<string, int> wireStates, List<LogicGate> gates)
    {
        var unresolvedGates = new Queue<LogicGate>(gates);
        var progressMade = false;

        do
        {
            progressMade = false;
            var remainingGates = new Queue<LogicGate>();

            foreach (var gate in unresolvedGates)
            {
                if (wireStates.TryGetValue(gate.Input1, out var input1Value) &&
                    wireStates.TryGetValue(gate.Input2, out var input2Value))
                {
                    wireStates[gate.Output] = gate.Type switch
                    {
                        "AND" => input1Value & input2Value,
                        "OR" => input1Value | input2Value,
                        "XOR" => input1Value ^ input2Value,
                        _ => throw new InvalidOperationException($"Unknown gate type: {gate.Type}")
                    };
                    progressMade = true;
                }
                else
                {
                    remainingGates.Enqueue(gate);
                }
            }

            unresolvedGates = remainingGates;
        } while (progressMade && unresolvedGates.Count > 0);

        Assert(unresolvedGates.Count <= 0, "Unresolvable gates detected, possible circular dependency!");
    }

    private static long CalculateOutputValue(Dictionary<string, int> wireStates)
    {
        // Combine values from wires starting with 'z' to form a binary number and convert it to a decimal number
        var binaryOutput = wireStates
            .Where(pair => pair.Key.StartsWith("z"))
            .OrderByDescending(pair => pair.Key)
            .Select(pair => pair.Value)
            .Aggregate("", (binary, value) => binary + value);

        return Convert.ToInt64(binaryOutput, 2);
    }

    private sealed record LogicGate(string Type, string Input1, string Input2, string Output);
}
