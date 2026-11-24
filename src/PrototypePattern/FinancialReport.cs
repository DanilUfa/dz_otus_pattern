namespace PrototypePattern;

public class FinancialReport : Report, IMyCloneable<FinancialReport>
{
    public string Currency { get; set; }
    public Dictionary<string, decimal> Amounts { get; } = [];

    public FinancialReport(string title, string author, string currency)
        : base(title, author)
    {
        Currency = currency;
    }

    protected FinancialReport(FinancialReport source) : base(source)
    {
        Currency = source.Currency;
        Amounts = new Dictionary<string, decimal>(source.Amounts, source.Amounts.Comparer);
    }

    public override FinancialReport Clone() => new(this);

    FinancialReport IMyCloneable<FinancialReport>.Clone() => Clone();
}
