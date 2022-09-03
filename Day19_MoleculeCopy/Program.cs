var instructions = new InputProvider<Instruction?>("Input.txt", GetInstruction).Where(w => w != null).Cast<Instruction>().ToList();

var inputMolecule = "CRnSiRnCaPTiMgYCaPTiRnFArSiThFArCaSiThSiThPBCaCaSiRnSiRnTiTiMgArPBCaPMgYPTiRnFArFArCaSiRnBPMgArPRnCaPTiRnFArCaSiThCaCaFArPBCaCaPTiTiRnFArCaSiRnSiAlYSiThRnFArArCaSiRnBFArCaCaSiRnSiThCaCaCaFYCaPTiBCaSiThCaSiThPMgArSiRnCaPBFYCaCaFArCaCaCaCaSiThCaSiRnPRnFArPBSiThPRnFArSiRnMgArCaFYFArCaSiRnSiAlArTiTiTiTiTiTiTiRnPMgArPTiTiTiBSiRnSiAlArTiTiRnPMgArCaFYBPBPTiRnSiRnMgArSiThCaFArCaSiThFArPRnFArCaSiRnTiBSiThSiRnSiAlYCaFArPRnFArSiThCaFArCaCaSiThCaCaCaSiRnPRnCaFArFYPMgArCaPBCaPBSiRnFYPBCaFArCaSiAl";

var transformator = new Transformator(instructions);

var allTransformations = transformator.GetAllPossibleTransformations(inputMolecule, 0, 1);

Console.WriteLine($"Part 1: {allTransformations.Count()}");

var reversedTransformator = new Transformator(instructions.Select(ReverseInstruction));

var toTransform = new PriorityQueue<(string, int), int>();
toTransform.Enqueue((inputMolecule, 0), 0);
string targetMolecule = "e";

while(true)
{
    (string molecule, int steps) = toTransform.Dequeue();

    var transformations = reversedTransformator.GetAllPossibleTransformations(molecule, 0, 1);

    if (transformations.Contains(targetMolecule))
    {
        Console.WriteLine($"Part 2: {steps + 1}");
        break;
    }

    foreach(var transformation in transformations)
    {
        toTransform.Enqueue((transformation, steps + 1), transformation.Length);
    }
}

static bool GetInstruction(string? input, out Instruction? value)
{
    value = null;

    if (input == null) return false;

    var parts = input.Split(new[] { " ", "=>" }, StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length != 2) throw new Exception();

    value = new Instruction(parts[0], parts[1]);

    return true;
}

static Instruction ReverseInstruction(Instruction instruction) =>
    new(instruction.After, instruction.Before);

class Transformator
{
    private readonly Dictionary<(string, int, int), IEnumerable<string>> memoizationDict = new();
    private readonly List<Instruction> instructions;

    public Transformator(IEnumerable<Instruction> instructions)
    {
        this.instructions = instructions.ToList();
    }

    public IEnumerable<string> GetAllPossibleTransformations(string input, int index, int replacementsLeft)
    {
        if (memoizationDict.ContainsKey((input, index, replacementsLeft)))
        {
            return memoizationDict[(input, index, replacementsLeft)];
        }
        else
        {
            var result = GetAllPossibleTransformationsInternal(input, index, replacementsLeft).ToHashSet();
            memoizationDict[(input, index, replacementsLeft)] = result;
            return result;
        }

        IEnumerable<string> GetAllPossibleTransformationsInternal(string input, int index, int replacementsLeft)
        {
            if (index >= input.Length)
            {
                if (replacementsLeft == 0) yield return input;
                else yield break;
            }
            else
            {
                //don't make the replacement, just pass through
                foreach (var str in GetAllPossibleTransformations(input, index + 1, replacementsLeft))
                    yield return str;

                //try making some replacements if avaliable
                if (replacementsLeft > 0)
                {
                    foreach (var instruction in instructions)
                    {
                        var newString = input;
                        var newIndex = index + 1;
                        var newReplacementLeft = replacementsLeft;

                        if (input[index..].StartsWith(instruction.Before))
                        {
                            newIndex = index + instruction.After.Length;

                            newString = input[..index] + instruction.After + input[(index + instruction.Before.Length)..];

                            newReplacementLeft--;
                        }

                        foreach (var str in GetAllPossibleTransformations(newString, newIndex, newReplacementLeft))
                            yield return str;
                    }
                }
            }
        }
    }
}

record Instruction (string Before, string After);