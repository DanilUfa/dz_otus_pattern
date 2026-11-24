namespace PrototypePattern;

public class Car : MotorVehicle, IMyCloneable<Car>
{
    public int Seats { get; set; }

    public Car(string name, int power, int seats) : base(name, power)
    {
        Seats = seats;
    }

    protected Car(Car source) : base(source)
    {
        Seats = source.Seats;
    }

    public override Car Clone() => new(this);

    Car IMyCloneable<Car>.Clone() => Clone();
}
