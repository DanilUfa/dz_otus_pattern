namespace PrototypePattern;

/// <summary>
/// Класс «Электромобиль» наследует <see cref="Car"/>.
/// Добавляет ёмкость батареи; топливо не используется.
/// </summary>
public class ElectricCar : Car, IMyCloneable<ElectricCar>, ICloneable
{
    public int BatteryCapacityKwh { get; }

    public ElectricCar(string name, int maxSpeed, int doors, int batteryCapacityKwh, IEnumerable<string>? extras = null)
        : base(name, maxSpeed, doors, "электричество", extras)
    {
        BatteryCapacityKwh = batteryCapacityKwh;
    }

    /// <summary>Copy-конструктор: цепочка родительских copy-конструкторов до базового класса.</summary>
    protected ElectricCar(ElectricCar other) : base(other)
    {
        BatteryCapacityKwh = other.BatteryCapacityKwh;
    }

    public override ElectricCar Clone() => new ElectricCar(this);

    object ICloneable.Clone() => Clone();
}
