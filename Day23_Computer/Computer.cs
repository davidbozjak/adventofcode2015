using System.Diagnostics;

class Computer
{
    private readonly UniqueFactory<string, Register> Registers;
    private readonly List<string> InstructionStrings;
    private int instructionIndex;

    public Computer(IEnumerable<string> instructions)
    {
        this.InstructionStrings = instructions.ToList();
        this.Registers = new UniqueFactory<string, Register>(name => new Register(name));
        this.instructionIndex = 0;
    }

    public void SetRegisterValue(string register, long value)
    {
        var r = Registers.GetOrCreateInstance(register);
        r.Value = value;
    }

    public long GetRegisterValue(string register)
    {
        var r = Registers.GetOrCreateInstance(register);
        return r.Value;
    }

    public void Run()
    {
        for (; instructionIndex >= 0 && instructionIndex < InstructionStrings.Count; instructionIndex++)
        {
            var instructionString = InstructionStrings[instructionIndex];

            var parts = instructionString.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            var instruction = parts[0];

            if (instruction == "hlf")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                r.Value /= 2;
            }
            else if (instruction == "tpl")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                r.Value *= 3;
            }
            else if (instruction == "inc")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                r.Value++;
            }
            else if (instruction == "jmp")
            {
                var value = GetValueOrRegisterValue(parts[1]);

                instructionIndex += (int)(value - 1);
            }
            else if (instruction == "jie")
            {
                var value = GetRegisterValue(parts[1]);

                if (value % 2 == 0)
                {
                    var offset = GetValueOrRegisterValue(parts[2]);

                    instructionIndex += (int)(offset - 1);
                }
            }
            else if (instruction == "jio")
            {
                var value = GetRegisterValue(parts[1]);

                if (value == 1)
                {
                    var offset = GetValueOrRegisterValue(parts[2]);

                    instructionIndex += (int)(offset - 1);
                }
            }
            else throw new Exception("Unknown instruction");
        }

        long GetValueOrRegisterValue(string valueOrRegister)
        {
            if (!long.TryParse(valueOrRegister, out long value))
            {
                var rx = Registers.GetOrCreateInstance(valueOrRegister);
                value = rx.Value;
            }

            return value;
        }
    }

    [DebuggerDisplay("{Name}:{Value}")]
    protected class Register
    {
        public string Name { get; }
        public long Value { get; set; } = 0;

        public Register(string name)
        {
            this.Name = name;
        }
    }
}