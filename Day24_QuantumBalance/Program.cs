var packages = new InputProvider<int>("Input.txt", int.TryParse).ToList();

var possibleGroups = GetGroupOfWeights(packages, packages.Sum() / 3);

//var balancedGroups = possibleGroups.Where(w => AreBalanced(w.g1, w.g2, w.g3)).ToList();

//var sorted = balancedGroups.Select(w => new { G1Count = w.g1.Count, QE = GetQuantumEntanglement(w.g1.GetElements()), G1 = w.g1, G2 = w.g2, G3 = w.g3 })
//    .OrderBy(w => w.G1Count)
//    .ThenBy(w => w.QE);

var balancedGroups = possibleGroups.Where(w => AreBalanced(w.g1, w.g2, w.g3));

var minQE = long.MaxValue;
var minElementCount = int.MaxValue;

foreach (var result in balancedGroups)
{
    var group = result.g1;

    if (group.Count <= minElementCount)
    {
        minElementCount = group.Count;

        var qe = GetQuantumEntanglement(result.g1.GetElements());

        if (qe < minQE)
        {
            Console.WriteLine($"Group with {result.g1.Count} elements with QE of {qe}");
            minQE = qe;
        }
    }
}

//Console.WriteLine($"QE: {sorted.First().QE}");

static long GetQuantumEntanglement(IEnumerable<int> group)
{
    long qe = 1;

    foreach (var element in group)
    {
        qe *= element;
    }

    return qe;
}

static bool AreBalanced(SummedSeries g1, SummedSeries g2, SummedSeries g3)
{
    return g1.Sum == g2.Sum && g1.Sum == g3.Sum;
}

static IEnumerable<(SummedSeries g1, SummedSeries g2, SummedSeries g3)> GetGroupOfWeights(IEnumerable<int> weights, int max)
{
    if (!weights.Any())
    {
        yield return (new SummedSeries(), new SummedSeries(), new SummedSeries());
        yield break;
    }

    var nextWeight = weights.First();
    var remainingWights = weights.Skip(1).ToList();

    foreach ((SummedSeries g1, SummedSeries g2, SummedSeries g3) in GetGroupOfWeights(remainingWights, max))
    {
        if (g1.Sum + nextWeight <= max)
            yield return (g1.Add(nextWeight), g2, g3);

        if (g2.Sum + nextWeight <= max)
            yield return (g1, g2.Add(nextWeight), g3);

        if (g3.Sum + nextWeight <= max)
            yield return (g1, g2, g3.Add(nextWeight));
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