namespace PrototypePattern;

public class Vehicle : IMyCloneable<Vehicle>, ICloneable
{
    public string Name { get; set; }
    public List<string> Accessories { get; }

    public Vehicle(string name)
    {
        Name = name;
        Accessories = [];
    }

    protected Vehicle(Vehicle source)
    {
        ArgumentNullException.ThrowIfNull(source);
        Name = source.Name;
        Accessories = new List<string>(source.Accessories);
    }

    public virtual Vehicle Clone() => new(this);

    // Потомки наследуют интерфейс; виртуальный вызов сохраняет их тип.
    object ICloneable.Clone() => Clone();
}
