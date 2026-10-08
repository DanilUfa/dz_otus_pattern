namespace PrototypePattern;

/// <summary>
/// Класс «Легковой автомобиль» наследует <see cref="Transport"/>.
/// Добавляет число дверей и тип топлива.
/// </summary>
public class Car : Transport, IMyCloneable<Car>, ICloneable
{
    public int Doors { get; }
    public string Fuel { get; }

    public Car(string name, int maxSpeed, int doors, string fuel, IEnumerable<string>? extras = null)
        : base(name, maxSpeed, extras)
    {
        Doors = doors;
        Fuel = fuel;
    }

    /// <summary>Copy-конструктор: сначала копирует родительскую часть, затем свою.</summary>
    protected Car(Car other) : base(other)
    {
        Doors = other.Doors;
        Fuel = other.Fuel;
    }

    /// <summary>Клонирование с ковариантным возвращаемым типом — результат всегда <see cref="Car"/>.</summary>
    public override Car Clone() => new Car(this);

    object ICloneable.Clone() => Clone();
}
