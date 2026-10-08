namespace PrototypePattern;

/// <summary>
/// Пользовательский дженерик-интерфейс для паттерна «Прототип».
/// В отличие от стандартного <see cref="ICloneable"/> возвращает
/// типизированную копию (без приведений) и подразумевает
/// «полную» (глубокую) копию объекта.
/// </summary>
/// <typeparam name="T">Тип клонируемого объекта.</typeparam>
public interface IMyCloneable<out T>
{
    /// <summary>Создаёт и возвращает копию текущего объекта.</summary>
    T Clone();
}
