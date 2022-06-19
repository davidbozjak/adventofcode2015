var instructionStrings = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();

var wires = new Dictionary<string, Wire>();

for (int i = 0; instructionStrings.Count > 0; i = (i + 1) % Math.Max(1, instructionStrings.Count))
{ 
    var instruction = instructionStrings[i];

    var parts = instruction.Split(new[] { " ", "->" }, StringSplitOptions.RemoveEmptyEntries);

    var wireName = parts.Last();

    if (wires.ContainsKey(wireName)) throw new Exception();

    if (parts.Length == 2)
    {
        if (ushort.TryParse(parts[0], out ushort signalValue))
        {
            wires[wireName] = new Wire(wireName, signalValue);
        }
        else
        {
            if (!wires.ContainsKey(parts[0])) continue;

            wires[wireName] = new Wire(wireName, wires[parts[0]]);
        }
    }
    else
    {
        BaseGate gate;

        if (parts.Length == 3)
        {
            if (parts[0] != "NOT") throw new Exception();

            if (!wires.ContainsKey(parts[1])) continue;

            gate = new NotGate();
            ((NotGate)gate).ConnectWire(wires[parts[1]]);
        }
        else if (parts.Length == 4)
        {
            Wire? wire0 = null;
            Wire? wire2 = null;

            if (ushort.TryParse(parts[0], out ushort directValue1))
            {
                wire0 = new Wire(parts[0], directValue1);
            }
            else if (!wires.ContainsKey(parts[0])) continue;

            if (ushort.TryParse(parts[2], out ushort directValue2))
            {
                wire2 = new Wire(parts[2], directValue2);
            }
            else if (!wires.ContainsKey(parts[2])) continue;

            if (parts[1] == "AND")
            {
                gate = new AndGate();
                ((AndGate)gate).ConnectWire(wire0 ?? wires[parts[0]]);
                ((AndGate)gate).ConnectWire(wire2 ?? wires[parts[2]]);
            }
            else if (parts[1] == "OR")
            {
                gate = new OrGate();
                ((OrGate)gate).ConnectWire(wire0 ?? wires[parts[0]]);
                ((OrGate)gate).ConnectWire(wire2 ?? wires[parts[2]]);
            }
            else if (parts[1] == "LSHIFT")
            {
                gate = new LeftShiftGate(ushort.Parse(parts[2]));
                ((LeftShiftGate)gate).ConnectWire(wire0 ?? wires[parts[0]]);
            }
            else if (parts[1] == "RSHIFT")
            {
                gate = new RightShiftGate(ushort.Parse(parts[2]));
                ((RightShiftGate)gate).ConnectWire(wire0 ?? wires[parts[0]]);
            }
            else throw new Exception();
        }
        else throw new Exception();

        wires[wireName] = new Wire(wireName, gate);
    }

    instructionStrings.Remove(instruction);
}

//foreach (var wire in wires.Values.OrderBy(w => w.Name))
//{
//    Console.WriteLine($"{wire.Name}: {wire.Signal}");
//}

Console.WriteLine($"Part 1: {wires["a"].Signal}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}