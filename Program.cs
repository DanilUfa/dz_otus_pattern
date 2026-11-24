using PrototypePattern;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var original = new ElectricCar("Электромобиль", 200, 5, 75);
original.Accessories.Add("Камера");
IMyCloneable<ElectricCar> prototype = original;
ElectricCar copy = prototype.Clone();
copy.Name = "Копия";
copy.Accessories.Add("Навигатор");
copy.BatteryCapacity = 90;
var standardCopy = (ElectricCar)((ICloneable)original).Clone();
Print("Оригинал", original);
Print("IMyCloneable<ElectricCar>", copy);
Print("ICloneable", standardCopy);

var checks = new (string Name, Action Run)[]
{
    ("Vehicle: оба интерфейса", () => VerifyBoth(new Vehicle("Транспорт"))),
    ("MotorVehicle: оба интерфейса", () => VerifyBoth(new MotorVehicle("Моторный транспорт", 100))),
    ("Car: оба интерфейса", () => VerifyBoth(new Car("Автомобиль", 150, 5))),
    ("ElectricCar: оба интерфейса", () => VerifyBoth(new ElectricCar("Электромобиль", 200, 5, 75))),
    ("Типизированные интерфейсы", VerifyTypedInterfaces),
    ("Базовые ссылки и ковариантность", VerifyPolymorphism),
    ("Независимость оригинала и двух копий", VerifyIndependence)
};
int failures = 0;
foreach (var (name, run) in checks)
{
    try
    {
        run();
        Console.WriteLine($"PASS: {name}");
    }
    catch (Exception error)
    {
        failures++;
        Console.Error.WriteLine($"FAIL: {name}: {error.Message}");
    }
}
Console.WriteLine($"Пройдено: {checks.Length - failures}/{checks.Length}");
return failures == 0 ? 0 : 1;

static void Print(string label, ElectricCar car) => Console.WriteLine(
    $"{label}: {car.Name}, {car.Power} л.с., {car.Seats} мест, " +
    $"{car.BatteryCapacity} кВт·ч; опции: {string.Join(", ", car.Accessories)}");

static void VerifyBoth(Vehicle source)
{
    source.Accessories.Add("Камера");
    VerifyCopy(source, ((IMyCloneable<Vehicle>)source).Clone());
    VerifyCopy(source, (Vehicle)((ICloneable)source).Clone());
}

static void VerifyCopy(Vehicle source, Vehicle clone)
{
    Check(!ReferenceEquals(source, clone), "Возвращён исходный объект");
    Check(source.GetType() == clone.GetType(), "Потерян фактический тип");
    Check(source.Name == clone.Name, "Название не скопировано");
    Check(source.Accessories.SequenceEqual(clone.Accessories), "Опции не скопированы");
    Check(!ReferenceEquals(source.Accessories, clone.Accessories), "Список опций общий");
    if (source is MotorVehicle motor && clone is MotorVehicle motorClone)
    {
        Check(motor.Power == motorClone.Power, "Мощность не скопирована");
        motorClone.Power++;
        Check(motor.Power != motorClone.Power, "Изменился оригинал");
    }
    if (source is Car car && clone is Car carClone)
    {
        Check(car.Seats == carClone.Seats, "Число мест не скопировано");
        carClone.Seats++;
        Check(car.Seats != carClone.Seats, "Изменился оригинал");
    }
    if (source is ElectricCar electric && clone is ElectricCar electricClone)
    {
        Check(electric.BatteryCapacity == electricClone.BatteryCapacity, "Батарея не скопирована");
        electricClone.BatteryCapacity++;
        Check(electric.BatteryCapacity != electricClone.BatteryCapacity, "Изменился оригинал");
    }
    clone.Name += " (копия)";
    clone.Accessories.Clear();
    Check(source.Name != clone.Name && source.Accessories.Count == 1, "Копия изменила оригинал");
}

static void VerifyTypedInterfaces()
{
    VerifyTypedCopy(new Vehicle("V"));
    VerifyTypedCopy(new MotorVehicle("M", 100));
    VerifyTypedCopy(new Car("C", 150, 5));
    VerifyTypedCopy(new ElectricCar("E", 200, 5, 75));
}

static void VerifyTypedCopy<T>(T source) where T : Vehicle, IMyCloneable<T>
{
    source.Accessories.Add("Камера");
    IMyCloneable<T> prototype = source;
    T clone = prototype.Clone();
    VerifyCopy(source, clone);
}

static void VerifyPolymorphism()
{
    var source = new ElectricCar("E", 200, 5, 75);
    source.Accessories.Add("Камера");
    Vehicle vehicle = source;
    MotorVehicle motor = source;
    Car car = source;
    IMyCloneable<ElectricCar> typed = source;
    IMyCloneable<Vehicle> covariant = typed;
    Vehicle[] clones = [vehicle.Clone(), motor.Clone(), car.Clone(), covariant.Clone(),
        ((IMyCloneable<MotorVehicle>)source).Clone(), ((IMyCloneable<Car>)source).Clone(),
        (Vehicle)((ICloneable)source).Clone()];
    foreach (var clone in clones)
    {
        VerifyCopy(source, clone);
    }
}

static void VerifyIndependence()
{
    var source = new ElectricCar("E", 200, 5, 75);
    source.Accessories.Add("Камера");
    var first = source.Clone();
    var second = source.Clone();
    source.Accessories.Clear();
    first.Accessories.Add("Навигатор");
    Check(second.Accessories.SequenceEqual(new[] { "Камера" }) && first.Accessories.Count == 2,
        "Копии используют общий список");
}

static void Check(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}
