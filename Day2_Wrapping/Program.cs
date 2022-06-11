using System.Text.RegularExpressions;

var boxes = new InputProvider<Box?>("Input.txt", GetBox).Where(w => w != null).Cast<Box>().ToList();

var wrappingPaperToOrder = boxes.Select(GetWrappingPaper).Sum();

Console.WriteLine($"Part 1: {wrappingPaperToOrder}");

var ribbonPaperToOrder = boxes.Select(GetRibbonLength).Sum();

Console.WriteLine($"Part 2: {ribbonPaperToOrder}");

static int GetWrappingPaper(Box box)
{
    var sides = new[] { box.Length * box.Width, box.Width * box.Height, box.Height * box.Length };

    var area = sides.Select(w => 2 * w).Sum();
    var slack = sides.Min();

    return area + slack;
}

static int GetRibbonLength(Box box)
{
    var perimeter = new[] { 2 * (box.Length + box.Width), 2 * (box.Width + box.Height), 2 * (box.Height+ box.Length) }.Min();
    var bow = box.Length * box.Width * box.Height;

    return perimeter + bow;
}

static bool GetBox(string? input, out Box? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");
    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    if (numbers.Length != 3) throw new Exception();

    value = new Box(numbers[0], numbers[1], numbers[2]);

    return true;
}

record Box (int Length, int Width, int Height);