class Wire
{
    public string Name { get; }
    public ushort Signal => initialSignal ?? Gate?.Signal ?? wire?.Signal ?? throw new Exception();

    private readonly ushort? initialSignal;
    private readonly IGate? Gate;
    private readonly Wire? wire;

    public Wire(string name, ushort signal)
    {
        this.Name = name;
        this.initialSignal = signal;
        this.Gate = null;
        this.wire = null;
    }
       
    public Wire(string name, IGate gate)
    {
        this.Name = name;
        this.Gate = gate;
        this.initialSignal = null;
        this.wire = null;
    }

    public Wire(string name, Wire wire)
    {
        this.Name = name;
        this.wire = wire;
        this.Gate = null;
        this.initialSignal = null;
    }
}
