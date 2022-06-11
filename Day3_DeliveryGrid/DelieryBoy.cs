using System.Drawing;

class DelieryBoy : IWorldObject
{
    private readonly UniqueFactory<(int, int), (int, int)> visitedCoordinates;

    public DelieryBoy()
    {
        this.visitedCoordinates = new(w => w);

        this.X = 0;
        this.Y = 0;
        this.visitedCoordinates.GetOrCreateInstance((X, Y));
    }

    public Point Position => new(X, Y);

    public char CharRepresentation => 'x';

    public int X { get; private set; }

    public int Y { get; private set; }

    public int Z => 1;

    public int NumberOfVisitedCoordinates => this.visitedCoordinates.AllCreatedInstances.Count;

    public IEnumerable<(int, int)> VisitedCoordinates => this.visitedCoordinates.AllCreatedInstances;

    public void MakeDelivery(char direction)
    {
        (X, Y) = direction switch
        {
            '<' => (X - 1, Y),
            '>' => (X + 1, Y),
            '^' => (X, Y - 1),
            'v' => (X, Y + 1),
            _ => throw new Exception()
        };

        var house = this.visitedCoordinates.GetOrCreateInstance((X, Y));
    }
}