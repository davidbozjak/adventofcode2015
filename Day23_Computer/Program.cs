var instructionStrings = new InputProvider<string?>("Input.txt", GetString).Cast<string>().ToList();

var computer = new Computer(instructionStrings);
computer.Run();

Console.WriteLine($"Part 1: {computer.GetRegisterValue("b")}");

computer = new Computer(instructionStrings);
computer.SetRegisterValue("a", 1);
computer.Run();

Console.WriteLine($"Part 2: {computer.GetRegisterValue("b")}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}