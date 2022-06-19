using System.Drawing;
using System.Text.RegularExpressions;

var instructions = new InputProvider<Instruction?>("Input.txt", GetString).Where(w => w != null).Cast<Instruction>().ToList();

var binaryBrightnessCells = new UniqueFactory<(int x, int y), BinaryBrightnessCell>(w => new BinaryBrightnessCell(w.x, w.y));

foreach (var instruction in instructions)
{
    for (int x = instruction.StartX; x <= instruction.EndX; x++)
        for (int y = instruction.StartY; y <= instruction.EndY; y++)
        {
            var cell = binaryBrightnessCells.GetOrCreateInstance((x, y));

            cell.IsOn = instruction.Op switch
            {
                Operation.TurnOn => true,
                Operation.TurnOff => false,
                Operation.Toggle => !cell.IsOn,
                _ => throw new Exception()
            };
        }
}

Console.WriteLine($"Part 1: {binaryBrightnessCells.AllCreatedInstances.Count(w => w.IsOn)}");

var spectrumBrightnessCells = new UniqueFactory<(int x, int y), SpectrumBrightnessCell>(w => new SpectrumBrightnessCell(w.x, w.y));

foreach (var instruction in instructions)
{
    for (int x = instruction.StartX; x <= instruction.EndX; x++)
        for (int y = instruction.StartY; y <= instruction.EndY; y++)
        {
            var cell = spectrumBrightnessCells.GetOrCreateInstance((x, y));

            cell.Brightness += instruction.Op switch
            {
                Operation.TurnOn => 1,
                Operation.TurnOff => -1,
                Operation.Toggle => 2,
                _ => throw new Exception()
            };

            cell.Brightness = Math.Max(0, cell.Brightness);
        }
}

Console.WriteLine($"Part 2: {spectrumBrightnessCells.AllCreatedInstances.Sum(w => w.Brightness)}");

static bool GetString(string? input, out Instruction? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");
    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    Operation op = input.Contains("toggle") ? Operation.Toggle :
        input.Contains("off") ? Operation.TurnOff : Operation.TurnOn;

    value = new Instruction(numbers[0], numbers[1], numbers[2], numbers[3], op);

    return true;
}

class BinaryBrightnessCell : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation => IsOn ? 'x' : '.';

    public int Z => 1;

    public bool IsOn { get; set; }

    public BinaryBrightnessCell(int x, int y)
    {
        this.Position = new Point(x, y);
        this.IsOn = false;
    }
}

class SpectrumBrightnessCell : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation => Brightness > 0 ? 'x' : '.'; 

    public int Z => 1;

    public long Brightness { get; set; }

    public SpectrumBrightnessCell(int x, int y)
    {
        this.Position = new Point(x, y);
        this.Brightness = 0; 
    }
}

enum Operation { TurnOn, TurnOff, Toggle};
record Instruction (int StartX, int StartY, int EndX, int EndY, Operation Op);