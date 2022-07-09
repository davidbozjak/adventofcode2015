var startingPass = "hxbxwxba";

int noOfPassToGenerate = 2;
for (string pass = IncrementString(startingPass); noOfPassToGenerate > 0; pass = IncrementString(pass))
{
    if (MatchesConditions(pass))
    {
        Console.WriteLine(pass);
        noOfPassToGenerate--;
    }
}

static bool MatchesConditions(string pass)
{
    var forbiddenLetters = new[] { 'i', 'o', 'l' };

    if (forbiddenLetters.Any(w => pass.Contains(w)))
        return false;

    if (!ContainsIncreasingStraight3(pass))
        return false;

    if (CountPairs(pass) < 2)
        return false;

    return true;
}

static bool ContainsIncreasingStraight3(string str)
{
    for (int i = 0; i < str.Length - 2; i++)
    {
        if (str[i] == str[i + 1] - 1 &&
            str[i + 1] == str[i + 2] - 1)
            return true;
    }

    return false;
}

static int CountPairs(string str)
{
    List<char> pairChars = new();

    for (int i = 0; i < str.Length - 1; i++)
    {
        if (str[i] == str[i + 1])
        {
            if (!pairChars.Contains(str[i]))
            {
                pairChars.Add(str[i]);
                i++; // must be non-overlapping
            }
        }
    }

    return pairChars.Count;
}

static string IncrementString(string str)
{
    var array = new char[str.Length];

    bool caryOver = true;

    for (int i = str.Length - 1; i >= 0; i--)
    {
        int c = str[i];

        if (caryOver)
        {
            c++;
            caryOver = false;
        }

        if (c > 'z')
        {
            c = 'a';
            caryOver = true;
        };

        array[i] = (char)c;
    }

    return new string(array);
}