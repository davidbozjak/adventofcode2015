var input = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();

Console.WriteLine($"Part 1: {input.Count(IsNice)}");

Console.WriteLine($"Part 2: {input.Count(IsNiceEx)}");

static bool IsNice(string str)
{
    var forbidden = new[] { "ab", "cd", "pq", "xy" };

    if (forbidden.Any(str.Contains)) return false;

    var vowels = "aeiou".ToCharArray();

    var noOfVowels = str.Count(vowels.Contains);

    if (noOfVowels < 3) return false;

    bool foundTwoLettersInARow = false;
    for (var i = 1; !foundTwoLettersInARow && i < str.Length; i++)
    {
        foundTwoLettersInARow = str[i] == str[i - 1];
    }

    return foundTwoLettersInARow;
}

static bool IsNiceEx(string str)
{
    bool foundSandwitchLetter = false;

    for (var i = 2; !foundSandwitchLetter && i < str.Length; i++)
    {
        foundSandwitchLetter = str[i] == str[i - 2];
    }

    if (!foundSandwitchLetter) return false;

    bool foundTwoNonOverlappingTwoLetters = false;

    for (var i = 2; !foundTwoNonOverlappingTwoLetters && i < str.Length; i++)
    {
        foundTwoNonOverlappingTwoLetters = str[i..].Contains(str[(i - 2)..i]);
    }

    return foundTwoNonOverlappingTwoLetters;
}

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}