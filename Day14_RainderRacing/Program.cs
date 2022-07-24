using System.Text.RegularExpressions;

var racers = new InputProvider<Racer?>("Input.txt", GetRacer).Where(w => w != null).Cast<Racer>().ToList();

int raceDuration = 2503;

for (int i = 0; i < raceDuration; i++)
{
    foreach (var racer in racers)
    {
        racer.MakeStep();
    }

    int maxDistance = racers.Select(w => w.Distance).Max();
    racers.Where(w => w.Distance == maxDistance).ToList().ForEach(w => w.AwardFirstPlaceForSecond());
}

Console.WriteLine($"Part 1: {racers.Select(w => w.Distance).Max()}");
Console.WriteLine($"Part 2: {racers.Select(w => w.SecondsWon).Max()}");

static bool GetRacer(string? input, out Racer? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");
    Regex nameRegex = new(@"[A-Z][a-z]*");

    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    value = new Racer(nameRegex.Match(input).Value, numbers[0], numbers[1], numbers[2]);

    return true;
}

class Racer
{
    public string Name { get; }

    public int Speed { get; }

    public int TotalRunDuration { get; }
    public int TotalRestDuration { get; }

    public int Distance { get; private set; } = 0;

    public int SecondsWon { get; private set; }

    private int remainingRunSeconds;
    private int remainingRestSeconds;

    public Racer (string name, int speed, int runDuration, int restDuration)
    {
        this.Name = name;
        this.Speed = speed;

        this.TotalRunDuration = runDuration;
        this.TotalRestDuration = restDuration;

        this.remainingRunSeconds = runDuration;
        this.remainingRestSeconds = 0;
    }

    public void MakeStep()
    {
        if (this.remainingRestSeconds > 0)
        {
            this.remainingRestSeconds--;

            if (this.remainingRestSeconds == 0)
            {
                this.remainingRunSeconds = this.TotalRunDuration;
            }
        }
        else if (this.remainingRunSeconds > 0)
        {
            this.Distance += this.Speed;
            this.remainingRunSeconds--;

            if (this.remainingRunSeconds == 0)
            {
                this.remainingRestSeconds = this.TotalRestDuration;
            }
        }
    }

    public void AwardFirstPlaceForSecond()
    {
        this.SecondsWon++;
    }
}