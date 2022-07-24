using System.Text.RegularExpressions;

var ingridients = new InputProvider<Ingridient?>("Input.txt", GetIngridient).Where(w => w != null).Cast<Ingridient>().ToList();

var allDistributions = EnumerateAllDistributionsOfX(ingridients.Count, 100);

var recipeScores = allDistributions.Select(w => EvaluateRecipe(ingridients, w));

Console.WriteLine($"Part 1: {recipeScores.Select(w => w.score).Max()}");
Console.WriteLine($"Part 2: {recipeScores.Where(w => w.calories == 500).Select(w => w.score).Max()}");

static IEnumerable<int[]> EnumerateAllDistributionsOfX(int numberOfParticipants, int X)
{
    if (numberOfParticipants == 1)
    {
        yield return new[] { X };
        yield break;
    }

    for (int i = 0; i <= X; i++)
    {
        var remainder = EnumerateAllDistributionsOfX(numberOfParticipants - 1, X - i);

        foreach (var r in remainder)
        {
            yield return r.Append(i).ToArray();
        }
    }
}

static bool GetIngridient(string? input, out Ingridient? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"-?\d+");

    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    value = new Ingridient(input[0..(input.IndexOf(":"))], numbers[0], numbers[1], numbers[2], numbers[3], numbers[4]);

    return true;
}

static (int score, int calories) EvaluateRecipe(IList<Ingridient> ingridients, int[] recipe)
{
    if (ingridients.Count != recipe.Length) throw new Exception();

    if (recipe.Sum() != 100) throw new Exception();

    int capacity = 0;
    int durability = 0;
    int flavor = 0;
    int texture = 0;
    int calories = 0;

    for (int i = 0; i < recipe.Length; i++)
    {
        capacity += recipe[i] * ingridients[i].Capacity;
        durability += recipe[i] * ingridients[i].Durability;
        flavor += recipe[i] * ingridients[i].Flavor;
        texture += recipe[i] * ingridients[i].Texture;
        calories += recipe[i] * ingridients[i].Calories;
    }

    int score = Math.Max(0, capacity) * Math.Max(0, durability) * Math.Max(0, flavor) * Math.Max(0, texture);

    return (score, calories);
}

record Ingridient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories);