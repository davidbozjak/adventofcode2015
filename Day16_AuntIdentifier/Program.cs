using System.Text.RegularExpressions;

var AllAunts = new InputProvider<AuntRecord?>("Input.txt", GetAuntRecord).Where(w => w != null).Cast<AuntRecord>().ToList();

IEnumerable<AuntRecord> selectedAunts = AllAunts.ToList();

selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "children", 3));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "cats", 7));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "samoyeds", 2));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "pomeranians", 3));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "akitas", 0));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "vizslas", 0));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "goldfish", 5));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "trees", 3));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "cars", 2));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "perfumes", 1));

Console.WriteLine($"Part 1: {selectedAunts.Count()} matches, first: {selectedAunts.First().Name}");

selectedAunts = AllAunts.ToList();

selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "children", 3));
selectedAunts = selectedAunts.Where(w => IsAtLeastOrUnknownForProperty(w, "cats", 7));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "samoyeds", 2));
selectedAunts = selectedAunts.Where(w => IsAtMostOrUnknownForProperty(w, "pomeranians", 3));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "akitas", 0));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "vizslas", 0));
selectedAunts = selectedAunts.Where(w => IsAtMostOrUnknownForProperty(w, "goldfish", 5));
selectedAunts = selectedAunts.Where(w => IsAtLeastOrUnknownForProperty(w, "trees", 3));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "cars", 2));
selectedAunts = selectedAunts.Where(w => IsExactOrUnknownForProperty(w, "perfumes", 1));

Console.WriteLine($"Part 2: {selectedAunts.Count()} matches, first: {selectedAunts.First().Name}");

static bool GetAuntRecord(string? input, out AuntRecord? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");

    var name = input[..input.IndexOf(":")];
    var propertiesStrings = input[(input.IndexOf(":")+1)..].Split(',', StringSplitOptions.RemoveEmptyEntries);
    var dict = propertiesStrings.ToDictionary(w => w[..w.IndexOf(":")].Trim(), w => int.Parse(numRegex.Match(w).Value));

    value = new AuntRecord(name, dict);

    return true;
}

static bool IsExactOrUnknownForProperty(AuntRecord aunt, string propertyName, int propertyValue)
{
    if (!aunt.Properties.ContainsKey(propertyName))
        return true;

    return aunt.Properties[propertyName] == propertyValue;
}

static bool IsAtLeastOrUnknownForProperty(AuntRecord aunt, string propertyName, int propertyValue)
{
    if (!aunt.Properties.ContainsKey(propertyName))
        return true;

    return aunt.Properties[propertyName] > propertyValue;
}

static bool IsAtMostOrUnknownForProperty(AuntRecord aunt, string propertyName, int propertyValue)
{
    if (!aunt.Properties.ContainsKey(propertyName))
        return true;

    return aunt.Properties[propertyName] < propertyValue;
}

record AuntRecord(string Name, Dictionary<string, int> Properties);