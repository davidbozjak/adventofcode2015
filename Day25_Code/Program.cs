long firstCode = 20151125;

var soughtIteration = GetIndexOfCoordinate(3019, 3010);

var code = GenerateCodeN(firstCode, soughtIteration);

Console.WriteLine(code);

static int GetIndexOfCoordinate(int soughtX, int soughtY)
{
    int noOfGenerated = 0;

    for (int diagnoal = 1; ; diagnoal++)
    {
        foreach (var coordinate in GetDiagnoalCoordinates(diagnoal))
        {
            if (coordinate.x == soughtX && coordinate.y == soughtY)
                return noOfGenerated;

            noOfGenerated++;
        }
    }

    static IEnumerable<(int x, int y)> GetDiagnoalCoordinates(int diagnoal)
    {
        for (int x = 1, y = diagnoal; y >= 1; x++, y--)
        {
            yield return (x, y);
        }
    }
}

static long GenerateCodeN(long firstCode, long N)
{
    long code = firstCode;

    for (int i = 0; i < N; i++)
    {
        code = GenerateCode(code);   
    }

    return code;
}

static long GenerateCode(long previousCode)
{
    return (previousCode * 252533) % 33554393;
}