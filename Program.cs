using System.Text;
using PrototypePattern;

namespace PrototypePatternLab;

/// <summary>
/// Демонстрация паттерна «Прототип»: клонирование через IMyCloneable&lt;T&gt;
/// и стандартный ICloneable, с автоматическими проверками корректности.
/// </summary>
internal static class Program
{
    private static readonly List<(bool Ok, string Title)> Checks = new();

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== Паттерн «Прототип»: демонстрация клонирования ===\n");

        // --- Базовая демонстрация: оригинал - клон - оригинал ---
        var tesla = new ElectricCar("Tesla Model 3", 220, 4, 75, new[] { "Автопилот", "Подогрев сидений" });
        tesla.Extras.Add("Аэродинамика");

        var teslaClone = tesla.Clone(); // IMyCloneable<ElectricCar>.Clone()
        Print("Оригинал: ", tesla);
        Print("Клон:     ", teslaClone);

        var bike = new Bicycle("Stels Navigator", 30, true, new[] { "Звонок" });
        var bikeClone = bike.Clone();
        Print("Велосипед:    ", bike);
        Print("Его клон:     ", bikeClone);

        Console.WriteLine();

        // --- Проверки IMyCloneable<T> ---
        Check("Тип клона электромобиля — ElectricCar (не Car и не Transport)",
            teslaClone.GetType() == typeof(ElectricCar));
        Check("Клон — другой объект (не та же ссылка)", !ReferenceEquals(tesla, teslaClone));
        Check("Имя скопировано", teslaClone.Name == tesla.Name);
        Check("Максимальная скорость скопирована", teslaClone.MaxSpeed == tesla.MaxSpeed);
        Check("Ёмкость батареи скопирована", teslaClone.BatteryCapacityKwh == tesla.BatteryCapacityKwh);
        Check("Список опций скопирован (глубокая копия)", teslaClone.Extras.SequenceEqual(tesla.Extras));
        Check("Список опций — отдельный объект", !ReferenceEquals(teslaClone.Extras, tesla.Extras));

        // изменение клона не должно влиять на оригинал (независимость ссылочного поля)
        teslaClone.Extras.Add("Новая опция");
        Check("Изменение клона не повлияло на оригинал",
            tesla.Extras.Count == 3 && teslaClone.Extras.Count == 4 && !teslaClone.Extras.SequenceEqual(tesla.Extras));

        // --- Ковариантность IMyCloneable<out T> ---
        IMyCloneable<Transport> prototype = tesla; // ElectricCar как базовый тип
        Transport fromBase = prototype.Clone();
        Check("Ковариантность: IMyCloneable<Transport> вернул фактически ElectricCar",
            fromBase.GetType() == typeof(ElectricCar));

        // --- Велосипед (вторая ветка иерархии) ---
        Check("Клон велосипеда — Bicycle", bikeClone.GetType() == typeof(Bicycle));
        Check("Корзина скопирована", bikeClone.HasBasket == bike.HasBasket);

        // --- Стандартный ICloneable: возвращает object, нужны приведение и проверка типа ---
        var ico = (ICloneable)new Car("Lada Vesta", 180, 4, "бензин", new[] { "Кондиционер" });
        object rawClone = ico.Clone();
        Check("ICloneable: объект вернулся, реальный тип — Car", rawClone.GetType() == typeof(Car));

        var electricAsCloneable = (ICloneable)tesla;
        var rawElectric = electricAsCloneable.Clone();
        Check("ICloneable: для электромобиля вернулся ElectricCar",
            rawElectric.GetType() == typeof(ElectricCar) &&
            ((ElectricCar)rawElectric).BatteryCapacityKwh == tesla.BatteryCapacityKwh);

        // --- Copy-конструкторы: цепочка клонирования от потомка до базы ---
        Check("Клон со всеми полями всех уровней иерархии",
            teslaClone.Doors == tesla.Doors &&
            teslaClone.Fuel == tesla.Fuel &&
            teslaClone.MaxSpeed == tesla.MaxSpeed &&
            teslaClone.BatteryCapacityKwh == tesla.BatteryCapacityKwh);

        Console.WriteLine();
        Console.WriteLine(Report());
    }

    private static void Print(string label, Transport item)
    {
        string extras = item.Extras.Count > 0 ? " [" + string.Join(", ", item.Extras) + "]" : "";
        Console.WriteLine($"{label}{item.GetType().Name}: {item.Name}, {item.MaxSpeed} км/ч{extras}");
    }

    private static void Check(string title, bool condition) => Checks.Add((condition, title));

    private static string Report()
    {
        int failed = Checks.Count(c => !c.Ok);
        var sb = new StringBuilder();
        sb.AppendLine("--- Результаты проверок ---");
        foreach (var c in Checks)
            sb.AppendLine($"{(c.Ok ? "[OK]  " : "[FAIL]")} {c.Title}");
        sb.AppendLine();
        sb.AppendLine($"Итого: {Checks.Count - failed} из {Checks.Count} проверок пройдено.");
        return sb.ToString();
    }
}
