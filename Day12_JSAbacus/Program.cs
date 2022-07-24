using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

var sum = new InputProvider<int>("Input.txt", GetSumÓfNumbers).Sum();

Console.WriteLine($"Part 1: {sum}");

var objects = JArray.Parse(System.IO.File.ReadAllText("Input.txt"));

var nonRedStrings = GetNonRedStrings(objects);

Console.WriteLine($"Part 2: {nonRedStrings.Select(GetSumÓfNumbersEx).Sum()}");

static bool GetSumÓfNumbers(string? input, out int value)
{
    value = 0;

    if (input == null) return false;

    value = GetSumÓfNumbersEx(input);

    return true;
}

static int GetSumÓfNumbersEx(string input)
{
    Regex numRegex = new(@"-?\d+");

    return numRegex.Matches(input).Select(w => int.Parse(w.Value)).Sum();
}

static IEnumerable<string> GetNonRedStrings(IEnumerable<JToken> input)
{
    var strings = new List<string>();

    foreach (var item in input)
    {
        if (item is JArray arrayItem)
        {
            strings.AddRange(GetNonRedStrings(arrayItem));
        }
        else if (item is JObject jobjectItem)
        {
            bool hasRed = false;

            var propertyValues = new List<string>();
            var children = new List<JObject>();

            foreach (var property in jobjectItem.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    children.Add(property.Value as JObject);
                    continue;
                }

                if (property.Value.ToString() == "red")
                {
                    hasRed = true;
                    break;
                }
                else
                {
                    propertyValues.Add(property.Value.ToString());
                }
            }

            if (!hasRed)
            {
                strings.AddRange(GetNonRedStrings(children));
                strings.AddRange(propertyValues);
            }
        }
        else if (item is JValue jvalue)
        {
            strings.Add(jvalue.ToString());
        }
        else
        {
            throw new Exception("Unexpected type");
        }
    }

    return strings;
}