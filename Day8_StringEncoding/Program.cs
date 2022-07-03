var parsedStrings = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();
var unescapedLengths = new List<int>();
var escapedLengths = new List<int>();

foreach (var str in parsedStrings)
{
    unescapedLengths.Add(GetInMemoryStringLength(str));
    escapedLengths.Add(GetEncodedStringLength(str));
}

Console.WriteLine($"Part 1: {parsedStrings.Sum(w => w.Length)} - {unescapedLengths.Sum()} = {parsedStrings.Sum(w => w.Length) - unescapedLengths.Sum()}");
Console.WriteLine($"Part 2: {escapedLengths.Sum()} - {parsedStrings.Sum(w => w.Length)} = {escapedLengths.Sum() - parsedStrings.Sum(w => w.Length)}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

static int GetInMemoryStringLength(string input)
{
    var memoryRepresentationLength = input.Length - 2;

    if (input[0] != '\"' ||
        input[^1] != '\"') throw new Exception("Assuming always starting and ending with quotes");

    for (int i = 1; i < input.Length - 2; i++)
    {
        if (input[i] == '\\' &&
            input[i + 1] == '\\')
        {
            memoryRepresentationLength--;
            i++;
        }
        else if (input[i] == '\\' &&
            input[i + 1] == '"')
        {
            memoryRepresentationLength--;
            i++;
        }
        else if (input.Length > (i + 3) && (input[i] == '\\' &&
            input[i + 1] == 'x' &&
            IsHexadecimalChar(input[i + 2]) &&
            IsHexadecimalChar(input[i + 3])))
        {
            memoryRepresentationLength -= 3;
            i += 3;
        }
    }

    return memoryRepresentationLength;
}

static int GetEncodedStringLength(string input)
{
    var codeRepresentationLength = input.Length + 2;

    if (input[0] != '\"' ||
        input[^1] != '\"') throw new Exception("Assuming always starting and ending with quotes");

    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] == '\\')
        {
            codeRepresentationLength++;
        }
        else if (input[i] == '\"')
        {
            codeRepresentationLength++;
        }
    }

    return codeRepresentationLength;
}

static bool IsHexadecimalChar(char c)
{
    return (c >= '0' && c <= '9') ||
        (c >= 'a' && c <= 'f');
}