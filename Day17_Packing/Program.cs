var containerSizes = new InputProvider<int>("Input.txt", int.TryParse).ToList();
var containers = Enumerable.Range(0, containerSizes.Count).Select(w => new Container(w, containerSizes[w]));

var memoDict = new Dictionary<(string, int), IEnumerable<IEnumerable<Container>?>>();

var options = GetPossibleContainersMemo(containers, 150).ToList();

Console.WriteLine($"Part 1: {options.Count}");

var minimumContainers = options.Min(w => w.Count());

Console.WriteLine($"Part 2: {options.Where(w => w.Count() == minimumContainers).Count()}");


IEnumerable<IEnumerable<Container>?> GetPossibleContainersMemo(IEnumerable<Container> avaliableContainers, int remaining)
{
    var avaliableContainersStr = string.Join(", ", avaliableContainers.Select(w => w.Id).OrderBy(w => w));

    if (!memoDict.ContainsKey((avaliableContainersStr, remaining)))
    {
        memoDict[(avaliableContainersStr, remaining)] = GetPossibleContainers(avaliableContainers, remaining).ToList();
    }

    return memoDict[(avaliableContainersStr, remaining)];
}

IEnumerable<IEnumerable<Container>?> GetPossibleContainers(IEnumerable<Container> avaliableContainers, int remaining)
{
    if (remaining == 0)
    {
        yield return Enumerable.Empty<Container>();
    }
    else if (avaliableContainers.Count() == 1)
    {
        var last = avaliableContainers.Last();
        if (last.Size != remaining) yield return null;
        else yield return new[] { last };
    }
    else
    {
        var containersUsedInOtherThreads = new List<Container>();

        foreach (var container in avaliableContainers)
        {
            containersUsedInOtherThreads.Add(container);

            if (container.Size > remaining) continue;
            
            int newRemaining = remaining - container.Size;
            var otherSuitableContainers = avaliableContainers.Except(containersUsedInOtherThreads);

            foreach(var path in GetPossibleContainersMemo(otherSuitableContainers, newRemaining))
            {
                if (path == null) continue;

                yield return path.Append(container);
            }
        }
    }

}

record Container(int Id, int Size);