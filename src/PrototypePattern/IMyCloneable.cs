namespace PrototypePattern;

/// <summary>Creates an independent deep copy preserving the runtime type.</summary>
public interface IMyCloneable<out T>
{
    T Clone();
}
