var directions = new InputProvider<char[]?>("Input.txt", GetChars).Where(w => w != null).Cast<char[]>().SelectMany(w => w).ToArray();

var santa = new DelieryBoy();

foreach (var direction in directions)
{
    santa.MakeDelivery(direction);
}

Console.WriteLine($"Part 1: {santa.NumberOfVisitedCoordinates}");

var deliverers = new[] { new DelieryBoy(), new DelieryBoy() };

for (int directionIndex = 0, delivererIndex = 0; directionIndex < directions.Length; directionIndex++, delivererIndex = (delivererIndex + 1) % deliverers.Length)
{
    deliverers[delivererIndex].MakeDelivery(directions[directionIndex]);
}

var uniqueVisitedCoordinates = deliverers.SelectMany(w => w.VisitedCoordinates).ToHashSet();

Console.WriteLine($"Part 2: {uniqueVisitedCoordinates.Count}");

static bool GetChars(string? input, out char[]? value)
{
    value = null;

    if (input == null) return false;

    value = input.ToCharArray();

    return true;
}