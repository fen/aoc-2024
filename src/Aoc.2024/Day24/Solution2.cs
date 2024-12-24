namespace Aoc.Day24;

public class Solution2 : ISolver
{
    public async ValueTask<string> SolveAsync(FileInfo inputFile)
    {
        var lines = await inputFile.ReadAllLinesAsync();
        return new CircuitSimulator(lines).IdentifyFaultyWires();
    }

    /// <summary>
    /// Represents the circuit device simulating logic gate operations.
    /// </summary>
    private sealed class CircuitSimulator
    {
        /// Stores the current state of wire signals.
        private Dictionary<string, bool> _currentWireStates;

        /// Original configuration of wires (unmodified state).
        private readonly Dictionary<string, bool> _initialWireStates;

        /// List of all gate operations in the circuit.
        private readonly List<Gate> _gates;

        public CircuitSimulator(string[] input)
        {
            _initialWireStates = new();
            _currentWireStates = new();
            _gates = [];

            ParseInput(input);
            SimulateCircuit();
        }

        /// <summary>
        /// Identifies faulty wires based on predefined rules.
        /// </summary>
        public string IdentifyFaultyWires()
        {
            var brokenWires = new HashSet<string>();
            var orGateInputs = new HashSet<string>();
            var andGateOutputs = new HashSet<string>();

            foreach (var gate in _gates)
            {
                // Rule 1: All wires starting with 'z' (except "z45") must be outputs of XOR gates.
                if (gate.OutputWire.StartsWith('z') && gate.GateType != LogicGateType.Xor && gate.OutputWire != "z45")
                {
                    brokenWires.Add(gate.OutputWire);
                }

                // Rule 2: XOR gates must have specific patterns for inputs or outputs.
                if (gate.GateType == LogicGateType.Xor &&
                    !(gate.InputWire1.StartsWith('x') || gate.InputWire1.StartsWith('y')) &&
                    !(gate.InputWire2.StartsWith('x') || gate.InputWire2.StartsWith('y')) &&
                    !gate.OutputWire.StartsWith('z'))
                {
                    brokenWires.Add(gate.OutputWire);
                }

                // Rule 3: Outputs of AND gates must serve as inputs to OR gates (except LSB x00 & y00).
                if (gate.GateType == LogicGateType.Or)
                {
                    orGateInputs.Add(gate.InputWire1);
                    orGateInputs.Add(gate.InputWire2);
                }
                else if (gate.GateType == LogicGateType.And &&
                    !(gate.InputWire1 == "x00" || gate.InputWire2 == "y00"))
                {
                    andGateOutputs.Add(gate.OutputWire);
                }
            }

            // Verify outputs of AND gates are properly used as inputs to OR gates.
            foreach (var input in orGateInputs)
            {
                if (!andGateOutputs.Contains(input))
                {
                    brokenWires.Add(input);
                }
                else
                {
                    andGateOutputs.Remove(input);
                }
            }

            brokenWires.UnionWith(andGateOutputs);

            return string.Join(",", brokenWires.OrderBy(w => w));
        }

        /// <summary>
        /// Parses the input to identify wire states and logic gate definitions.
        /// </summary>
        private void ParseInput(string[] input)
        {
            var parsingInitialSignals = true;

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    parsingInitialSignals = false;
                    continue;
                }

                var split = line.Split(' ');

                if (parsingInitialSignals)
                {
                    var wire = split[0][..^1]; // Remove trailing colon (:)
                    var signal = split[1] == "1";
                    _initialWireStates[wire] = signal;
                }
                else
                {
                    var inputWire1 = split[0];
                    var operation = split[1];
                    var inputWire2 = split[2];
                    var outputWire = split[4];

                    var gateType = operation switch
                    {
                        "AND" => LogicGateType.And,
                        "OR" => LogicGateType.Or,
                        "XOR" => LogicGateType.Xor,
                        _ => throw new InvalidOperationException("Unknown operation.")
                    };

                    _gates.Add(new Gate(inputWire1, inputWire2, outputWire, gateType));
                }
            }
        }

        /// <summary>
        /// Simulates the operation of the entire circuit.
        /// </summary>
        private void SimulateCircuit()
        {
            _currentWireStates = new Dictionary<string, bool>(_initialWireStates);
            var pendingGates = new List<Gate>(_gates);

            while (pendingGates.Count > 0)
            {
                var skippedGates = new List<Gate>();

                foreach (var gate in pendingGates)
                {
                    if (!ProcessGate(gate))
                    {
                        skippedGates.Add(gate);
                    }
                }

                // Replace pending-gates with skipped ones for the next iteration.
                pendingGates = skippedGates;
            }
        }

        /// <summary>
        /// Processes a single gate operation, updating the circuit state.
        /// </summary>
        private bool ProcessGate(Gate gate)
        {
            if (_currentWireStates.TryGetValue(gate.InputWire1, out var signal1) &&
                _currentWireStates.TryGetValue(gate.InputWire2, out var signal2))
            {
                _currentWireStates[gate.OutputWire] = gate.GateType switch
                {
                    LogicGateType.And => signal1 && signal2,
                    LogicGateType.Or => signal1 || signal2,
                    LogicGateType.Xor => signal1 ^ signal2,
                    _ => throw new InvalidOperationException("Invalid gate type.")
                };

                return true;
            }

            return false;
        }
    }

    private sealed class Gate(string inputWire1, string inputWire2, string outputWire, LogicGateType gateType)
    {
        public string InputWire1 { get; } = inputWire1;
        public string InputWire2 { get; } = inputWire2;
        public string OutputWire { get; } = outputWire;
        public LogicGateType GateType { get; } = gateType;
    }

    private enum LogicGateType
    {
        Or,
        Xor,
        And
    }
}
