var packages = new InputProvider<int>("Input.txt", int.TryParse).ToList();

Console.WriteLine($"Part 1:");
GenerateNGroupsAndFindLowQE(packages, 3);

Console.WriteLine($"Part 2:");
GenerateNGroupsAndFindLowQE(packages, 4);

static void GenerateNGroupsAndFindLowQE(IEnumerable<int> elements, int numberOfGroups)
{
    var possibleGroups = GetGroupOfWeights(elements, elements.Sum() / numberOfGroups, numberOfGroups);

    var balancedGroups = possibleGroups.Where(w => AreBalanced(w));

    var minQE = long.MaxValue;
    var minElementCount = int.MaxValue;

    foreach (var result in balancedGroups)
    {
        var group = result.First();

        if (group.Count <= minElementCount)
        {
            if (group.Count < minElementCount)
            {
                minQE = long.MaxValue;
            }

            minElementCount = group.Count;

            var qe = GetQuantumEntanglement(group.GetElements());

            if (qe < minQE)
            {
                Console.WriteLine($"Group with {group.Count} elements with QE of {qe}");
                minQE = qe;
            }
        }
    }
}

static long GetQuantumEntanglement(IEnumerable<int> group)
{
    long qe = 1;

    foreach (var element in group)
    {
        qe *= element;
    }

    return qe;
}

static bool AreBalanced(IEnumerable<SummedSeries> series)
{
    var first = series.First();
    return series.All(w => w.Sum == first.Sum);
}

static IEnumerable<List<SummedSeries>> GetGroupOfWeights(IEnumerable<int> weights, int max, int numberOfGroups)
{
    if (!weights.Any())
    {
        yield return Enumerable.Range(0, numberOfGroups).Select(w => new SummedSeries()).ToList();
        yield break;
    }

    var nextWeight = weights.First();
    var remainingWights = weights.Skip(1).ToList();

    foreach (var groups in GetGroupOfWeights(remainingWights, max, numberOfGroups))
    {
        foreach (var group in groups.OrderByDescending(w => w.Count))
        {
            if (group.Sum + nextWeight <= max)
            {
                var newGroup = group.Add(nextWeight);

                yield return groups.Where(w => w != group).Append(newGroup).OrderBy(w => w.Count).ToList();
            }
        }
    }
}

class SummedSeries
{
    private readonly SummedSeries? subseries;
    private readonly int? element;

    public int Sum { get; }

    public int Count { get; }

    public SummedSeries()
    {
        this.element = null;
        this.subseries = null;
        this.Sum = 0;
        this.Count = 0;
    }

    private SummedSeries(int element, SummedSeries subseries)
    {
        this.subseries = subseries;
        this.element = element;
        this.Sum = element + this.subseries.Sum;
        this.Count = 1 + this.subseries.Count;
    }

    public SummedSeries Add(int element)
    {
        return new SummedSeries(element, this);
    }

    public IEnumerable<int> GetElements()
    {
        if (this.element.HasValue)
        {
            yield return this.element.Value;
        }

        if (this.subseries != null)
        {
            foreach (var e in this.subseries.GetElements())
            {
                yield return e;
            }
        }
    }
}