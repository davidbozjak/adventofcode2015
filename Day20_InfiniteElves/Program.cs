var thresholdNumber = 34000000;

var maxGifts10AtHouse = int.MinValue;
var maxGifts11AtHouse = int.MinValue;

//start at 2 for complete solution, for time execution reasons skip obviously too early houses
for (int house = 780000; maxGifts10AtHouse < thresholdNumber || maxGifts11AtHouse < thresholdNumber; house++)
{
    int gifts10AtHouse = 10;
    int gifts11AtHouse = 11;

    for (int elf = 2; elf <= house; elf++)
    {
        if (house % elf != 0) continue;

        gifts10AtHouse += elf * 10;

        if (house > elf * 50)
            continue;

        gifts11AtHouse += elf * 11;
    }

    if (gifts10AtHouse > maxGifts10AtHouse)
    {
        maxGifts10AtHouse = gifts10AtHouse;
        Console.WriteLine($"Part 1: House {house} gets {gifts10AtHouse} gifts");
    }

    if (gifts11AtHouse > maxGifts11AtHouse)
    {
        maxGifts11AtHouse = gifts11AtHouse;
        Console.WriteLine($"Part 2: House {house} gets {gifts11AtHouse} gifts");
    }
}