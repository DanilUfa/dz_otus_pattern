namespace PrototypePattern;

/// <summary>Создаёт независимую глубокую копию с сохранением фактического типа.</summary>
public interface IMyCloneable<out T>
{
    T Clone();
}
