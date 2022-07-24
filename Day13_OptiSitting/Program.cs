using System.Text.RegularExpressions;

var sittingPairs = new InputProvider<SittingPair?>("Input.txt", GetPair).Where(w => w != null).Cast<SittingPair>().ToList();

var effects = sittingPairs.ToDictionary(w => (w.Person, w.Neighbour), w => w.Effect);

var people = sittingPairs.SelectMany(w => new[] { w.Person, w.Neighbour }).ToHashSet().ToList();

Console.WriteLine($"Part 1: {GetMaxHappinessForPeople(people, effects)}");

people.Add("DavidB");

Console.WriteLine($"Part 2: {GetMaxHappinessForPeople(people, effects)}");

int GetMaxHappinessForPeople(List<string> people, Dictionary<(string, string), int> effects)
{
    var allSittingOrders = people.GetAllOrdersOfList();

    var evaluatedSittingOrders = allSittingOrders
        .Select(w => new { SittingOrder = w, Value = EvaluateSittingOrder(w.ToList(), effects) })
        .OrderByDescending(w => w.Value);

    return evaluatedSittingOrders.First().Value;
}

static bool GetPair(string? input, out SittingPair? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");
    Regex nameRegex = new(@"[A-Z][a-z]*");

    var diff = int.Parse(numRegex.Match(input).Value) * (input.Contains("lose") ? -1 : 1);
    var names = nameRegex.Matches(input).Select(w => w.Value).ToArray();

    value = new SittingPair(names[0], names[1], diff);

    return true;
}

static int EvaluateSittingOrder(IReadOnlyList<string> sittingOrder, Dictionary<(string, string), int> effects)
{
    int eval = 0;

    for (int i = 0; i < sittingOrder.Count; i++)
    {
        eval += GetEffectOrZero(i, i - 1);
        eval += GetEffectOrZero(i, i + 1);
    }

    return eval;

    int GetEffectOrZero(int personIndex, int neighbourIndex)
    {
        neighbourIndex = neighbourIndex >= 0 ? neighbourIndex : sittingOrder.Count - 1;
        neighbourIndex = neighbourIndex <= sittingOrder.Count - 1 ? neighbourIndex : 0;

        if (effects.ContainsKey((sittingOrder[personIndex], sittingOrder[neighbourIndex])))
        {
            return effects[(sittingOrder[personIndex], sittingOrder[neighbourIndex])];
        }
        else
        {
            return 0;
        }
    }
}

record SittingPair(string Person, string Neighbour, int Effect);