interface IGate
{
    public ushort Signal { get; }
}

abstract class BaseGate : IGate
{
    private readonly Cached<ushort> cachedSignal;

    public ushort Signal => this.cachedSignal.Value;

    public BaseGate()
    {
        this.cachedSignal = new Cached<ushort>(this.CalculateSignal);
    }

    protected abstract ushort CalculateSignal();
}

abstract class BinaryBaseGate : BaseGate
{
    private readonly List<Wire> wires = new();
    
    public void ConnectWire(Wire wire)
    {
        if (this.wires.Count >= 2) throw new Exception("Max 2 inputs");

        this.wires.Add(wire);
    }

    protected override ushort CalculateSignal()
    {
        if (wires.Count != 2) throw new Exception("Expected exactly two inputs");
        return this.CalculateSignal(this.wires[0].Signal, this.wires[1].Signal);
    }

    protected abstract ushort CalculateSignal(ushort firstValue, ushort secondValue);
}

abstract class UniaryBaseGate : BaseGate
{
    private readonly List<Wire> wires = new();

    public void ConnectWire(Wire wire)
    {
        if (this.wires.Count >= 1) throw new Exception("Max 2 inputs");

        this.wires.Add(wire);
    }

    protected override ushort CalculateSignal()
    {
        if (wires.Count != 1) throw new Exception("Expected exactly one input");
        return this.CalculateSignal(this.wires[0].Signal);
    }

    protected abstract ushort CalculateSignal(ushort value);
}

class AndGate : BinaryBaseGate
{
    protected override ushort CalculateSignal(ushort firstValue, ushort secondValue)
    {
        return (ushort)(firstValue & secondValue);
    }
}

class OrGate : BinaryBaseGate
{
    protected override ushort CalculateSignal(ushort firstValue, ushort secondValue)
    {
        return (ushort)(firstValue | secondValue);
    }
}



class NotGate : UniaryBaseGate
{
    protected override ushort CalculateSignal(ushort value)
    {
        return (ushort)(~value);
    }
}

class RightShiftGate : UniaryBaseGate
{
    private readonly ushort numPlaces;

    public RightShiftGate(ushort numPlaces)
    {
        this.numPlaces = numPlaces;
    }

    protected override ushort CalculateSignal(ushort value)
    {
        return (ushort)(value >> numPlaces);
    }
}

class LeftShiftGate : UniaryBaseGate
{
    private readonly ushort numPlaces;

    public LeftShiftGate(ushort numPlaces)
    {
        this.numPlaces = numPlaces;
    }

    protected override ushort CalculateSignal(ushort value)
    {
        return (ushort)(value << numPlaces);
    }
}