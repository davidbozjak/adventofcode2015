using System.Drawing;
using System.Text.RegularExpressions;

Regex numRegex = new(@"\d+");
Regex hexColorRegex = new(@"#[0-9a-z][0-9a-z][0-9a-z][0-9a-z][0-9a-z][0-9a-z]");

var wholeStringInput = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();
var cells = new List<Cell>();

for (int y = 0; y < wholeStringInput.Count; y++)
{
    for (int x = 0; x < wholeStringInput[y].Length; x++)
    {
        var cell = new Cell(x, y);
        cell.OverrideState(wholeStringInput[y][x] == '#');
        cells.Add(cell);
    }
}

//override corners to be ON
var lockedCells = new List<Cell>() 
{
    cells.Where(w => w.X == 0 && w.Y == 0).First(),
    cells.Where(w => w.X == wholeStringInput[0].Length - 1 && w.Y == 0).First(),
    cells.Where(w => w.X == 0 && w.Y == wholeStringInput.Count - 1).First(),
    cells.Where(w => w.X == wholeStringInput[0].Length - 1 && w.Y == wholeStringInput.Count - 1).First()
};

lockedCells.ForEach(w => w.OverrideState(true));

foreach (var cell in cells)
{
    cell.SetNeighbours(cells.Where(w => w != cell && Math.Abs(cell.X - w.X) <= 1 && Math.Abs(cell.Y - w.Y) <= 1));
}

var processableCells = cells.Except(lockedCells).ToList();

for (int i = 0; i < 100; i++)
{
    processableCells.ForEach(w => w.ReevaluateState());
    processableCells.ForEach(w => w.UpdateState());

    Console.WriteLine($"Generation {i + 1}: {cells.Count(w => w.IsOn)} lights are on.");
}


static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

class Cell : IWorldObject
{
    public bool IsOn { get; private set; }

    public Point Position { get; }

    public char CharRepresentation => this.IsOn ? '#' : '.';

    public int X => this.Position.X;

    public int Y => this.Position.Y;

    public int Z => 0;

    private IEnumerable<Cell>? neighbours;
    private bool? newState;

    public Cell(int x, int y)
    {
        this.Position = new Point(x, y);
    }

    public void SetNeighbours(IEnumerable<Cell> neighbours)
    {
        var list = new List<Cell>();

        foreach (var cell in neighbours)
        {
            if (cell.Position == this.Position)
                throw new Exception();

            list.Add(cell);
        }

        this.neighbours = list;
    }

    public void OverrideState(bool isOn)
    {
        this.IsOn = isOn;
    }

    public void ReevaluateState()
    {
        if (this.neighbours == null)
            throw new Exception();

        newState = this.IsOn;

        var numberOfNeighboursOn = this.neighbours.Count(w => w.IsOn);

        if (this.IsOn)
        {
            newState = (numberOfNeighboursOn == 2 || numberOfNeighboursOn == 3) ? true : false;
        }
        else
        {
            newState = numberOfNeighboursOn == 3 ? true : false;
        }
    }

    public void UpdateState()
    {
        if (newState == null) throw new Exception();

        this.IsOn = newState.Value;
        newState = null;
    }
}