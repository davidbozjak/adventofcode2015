class Wire
{
    public string Name { get; }
    public ushort Signal => overrideValue ?? initialSignal ?? gate?.Signal ?? wire?.Signal ?? throw new Exception();

    private ushort? overrideValue;
    private readonly ushort? initialSignal;
    private readonly IGate? gate;
    private readonly Wire? wire;
    

    public Wire(string name, ushort signal)
    {
        this.Name = name;
        this.initialSignal = signal;
        this.gate = null;
        this.wire = null;
    }
       
    public Wire(string name, IGate gate)
    {
        this.Name = name;
        this.gate = gate;
        this.initialSignal = null;
        this.wire = null;
    }

    public Wire(string name, Wire wire)
    {
        this.Name = name;
        this.wire = wire;
        this.gate = null;
        this.initialSignal = null;
    }

    public void Reset()
    {
        this.wire?.Reset();
        this.gate?.Reset();
        this.overrideValue = null;
    }

    public void SetOverrideValue(ushort overrideValue)
    {
        this.overrideValue = overrideValue;
    }
}
