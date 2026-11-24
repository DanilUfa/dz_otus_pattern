namespace PrototypePattern;

public class MotorVehicle : Vehicle, IMyCloneable<MotorVehicle>
{
    public int Power { get; set; }

    public MotorVehicle(string name, int power) : base(name)
    {
        Power = power;
    }

    protected MotorVehicle(MotorVehicle source) : base(source)
    {
        Power = source.Power;
    }

    public override MotorVehicle Clone() => new(this);

    MotorVehicle IMyCloneable<MotorVehicle>.Clone() => Clone();
}
