using System.Text;

var value = "3113322113";

for (int i = 0; i < 50; i++)
{
    value = GenerateLookAndSay(value);
    Console.WriteLine($"{i + 1}: {value.Length}");
}

static string GenerateLookAndSay(string input)
{
    var builder = new StringBuilder();

    var repeatedCount = 1;

    for (int i = 0; i < input.Length - 1; i++)
    {
        if (input[i] == input[i + 1])
        {
            repeatedCount++;
        }
        else
        {
            builder.Append(repeatedCount);
            builder.Append(input[i]);
            repeatedCount = 1;
        }
    }

    builder.Append(repeatedCount);
    builder.Append(input[^1]);

    return builder.ToString();
}