namespace PrototypePattern;

/// <summary>
/// Базовый класс «Транспортное средство».
/// Общие свойства: название, максимальная скорость и список доп. опций.
/// Список опций — ссылочное поле, поэтому при клонировании нужна его
/// отдельная (глубокая) копия.
/// </summary>
public class Transport : IMyCloneable<Transport>, ICloneable
{
    public string Name { get; }
    public int MaxSpeed { get; }
    public List<string> Extras { get; }

    public Transport(string name, int maxSpeed, IEnumerable<string>? extras = null)
    {
        Name = name;
        MaxSpeed = maxSpeed;
        Extras = extras?.ToList() ?? new List<string>();
    }

    /// <summary>Copy-конструктор: используется для клонирования.</summary>
    protected Transport(Transport other)
    {
        Name = other.Name;
        MaxSpeed = other.MaxSpeed;
        Extras = new List<string>(other.Extras); // глубокая копия ссылочного поля
    }

    /// <summary>Создаёт копию объекта через copy-конструктор.</summary>
    public virtual Transport Clone() => new Transport(this);

    /// <summary>Реализация стандартного интерфейса через собственный метод Clone.</summary>
    object ICloneable.Clone() => Clone();
}
