var distanceRecords = new InputProvider<DistanceRecord?>("Input.txt", GetDistanceRecord).Where(w => w != null).Cast<DistanceRecord>().ToList();

var locations = new HashSet<string>();
var distances = new Dictionary<(string, string), int>();

foreach (var record in distanceRecords)
{
    locations.Add(record.LocationA);
    locations.Add(record.LocationB);

    distances[(record.LocationA, record.LocationB)] = record.Distance;
    distances[(record.LocationB, record.LocationA)] = record.Distance;
}

var minDistances = new List<int>();
var maxDistances = new List<int>();

foreach (var startLocation in locations)
{
    minDistances.Add(GetBestDistanceForEval(startLocation, locations.Where(w => w != startLocation).ToList(), (distance, currentBest) => distance < currentBest));
    maxDistances.Add(GetBestDistanceForEval(startLocation, locations.Where(w => w != startLocation).ToList(), (distance, currentBest) => distance > currentBest));
}

Console.WriteLine($"Part 1: {minDistances.Min()}");
Console.WriteLine($"Part 1: {maxDistances.Max()}");

int GetBestDistanceForEval(string location, List<string> locationsToVisit, Func<int, int, bool> eval)
{
    if (locationsToVisit.Count == 0) return 0;

    int? currentBest = null;

    foreach (var targetLocation in locationsToVisit)
    {
        var distance = distances[(location, targetLocation)];

        var totalDistance = distance + GetBestDistanceForEval(targetLocation, locationsToVisit.Where(w => w != targetLocation).ToList(), eval);

        if (currentBest == null || eval(totalDistance, currentBest.Value))
        {
            currentBest = totalDistance;
        }
    }

    return currentBest.HasValue ? currentBest.Value : throw new Exception();
}

static bool GetDistanceRecord(string? input, out DistanceRecord? value)
{
    value = null;

    if (input == null) return false;

    var parts = input.Split(new[] { " to ", " = ", " " }, StringSplitOptions.RemoveEmptyEntries);

    value = new DistanceRecord(parts[0], parts[1], int.Parse(parts[2]));

    return true;
}

record DistanceRecord(string LocationA, string LocationB, int Distance);