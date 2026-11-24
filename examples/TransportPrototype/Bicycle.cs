namespace PrototypePattern;

/// <summary>
/// Класс «Велосипед» наследует <see cref="Transport"/> — вторая ветка иерархии.
/// Добавляет признак наличия корзины.
/// </summary>
public class Bicycle : Transport, IMyCloneable<Bicycle>, ICloneable
{
    public bool HasBasket { get; }

    public Bicycle(string name, int maxSpeed, bool hasBasket, IEnumerable<string>? extras = null)
        : base(name, maxSpeed, extras)
    {
        HasBasket = hasBasket;
    }

    /// <summary>Copy-конструктор.</summary>
    protected Bicycle(Bicycle other) : base(other)
    {
        HasBasket = other.HasBasket;
    }

    public override Bicycle Clone() => new Bicycle(this);

    object ICloneable.Clone() => Clone();
}
