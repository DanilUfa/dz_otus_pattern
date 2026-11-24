namespace PrototypePattern;

public sealed class ElectricCar : Car, IMyCloneable<ElectricCar>
{
    public int BatteryCapacity { get; set; }

    public ElectricCar(string name, int power, int seats, int batteryCapacity)
        : base(name, power, seats)
    {
        BatteryCapacity = batteryCapacity;
    }

    private ElectricCar(ElectricCar source) : base(source)
    {
        BatteryCapacity = source.BatteryCapacity;
    }

    public override ElectricCar Clone() => new(this);

    ElectricCar IMyCloneable<ElectricCar>.Clone() => Clone();
}
