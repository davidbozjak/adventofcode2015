var input = new InputProvider<string?>("Input.txt", GetString).First();

int current = 0;
int position = 1;

foreach (char c in input)
{
    current += c switch
    {
        '(' => 1,
        ')' => -1,
        _ => throw new Exception()
    };

    if (current == -1)
    {
        Console.WriteLine($"Part 2: {position}");
    }

    position++;
}

Console.WriteLine($"Part 1: {current}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}