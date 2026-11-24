namespace PrototypePattern;

public sealed class AnnualFinancialReport : FinancialReport, IMyCloneable<AnnualFinancialReport>
{
    public int Year { get; set; }
    public List<string> AuditNotes { get; } = [];

    public AnnualFinancialReport(string title, string author, string currency, int year)
        : base(title, author, currency)
    {
        Year = year;
    }

    private AnnualFinancialReport(AnnualFinancialReport source) : base(source)
    {
        Year = source.Year;
        AuditNotes = new List<string>(source.AuditNotes);
    }

    public override AnnualFinancialReport Clone() => new(this);

    AnnualFinancialReport IMyCloneable<AnnualFinancialReport>.Clone() => Clone();
}
