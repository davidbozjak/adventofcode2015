using System.Text.RegularExpressions;

var parsedStrings = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();
var unescapedLengths = new List<int>();

foreach (var str in parsedStrings)
{
    unescapedLengths.Add(GetStringLength(str));
}

Console.WriteLine($"{parsedStrings.Sum(w => w.Length)} - {unescapedLengths.Sum()} = {parsedStrings.Sum(w => w.Length) - unescapedLengths.Sum()}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

//static int GetStringLength(string input)
//{
//    var length = input.Length - 2;

//    if (input[0] != '\"' ||
//        input[^1] != '\"') throw new Exception("Assuming always starting and ending with quotes");

//    var backlashRegex = new Regex(@"\\\\");
//    length -= backlashRegex.Matches(input).Count;

//    var quotesRegex = new Regex("\\\"");
//    length -= quotesRegex.Matches(input).Count;

//    var asciiRegex = new Regex(@"\\x[0-9a-fA-F][0-9a-fA-F]");
//    length -= asciiRegex.Matches(input).Count * 3;

//    Console.WriteLine(length);

//    return length;
//}

static int GetStringLength(string input)
{
    var length = input.Length - 2;

    if (input[0] != '\"' ||
        input[^1] != '\"') throw new Exception("Assuming always starting and ending with quotes");

    for (int i = 1; i < input.Length - 2; i++)
    {
        if (input[i] == '\\' &&
            input[i + 1] == '\\')
        {
            length--;
            i++;
        }
        else if (input[i] == '\\' &&
            input[i + 1] == '"')
        {
            length--;
            i++;
        }
        else if (input.Length > (i + 3) && (input[i] == '\\' &&
            input[i + 1] == 'x' &&
            IsHexadecimalChar(input[i + 2]) &&
            IsHexadecimalChar(input[i + 3])))
        {
            length -= 3;
            i += 3;
        }
    }

    Console.WriteLine(input);
    Console.WriteLine($"{input.Length} - {length}");
    //Console.ReadKey();

    return length;
}

static bool IsHexadecimalChar(char c)
{
    return (c >= '0' && c <= '9') ||
        //(c >= 'A' && c <= 'F') ||
        (c >= 'a' && c <= 'f');
}