using PrototypePattern;

var original = new AnnualFinancialReport("Annual report", "Danil", "RUB", 2026);
original.Tags.Add("finance");
original.Sections.Add("Revenue");
original.Amounts.Add("Revenue", 1_000_000m);
original.AuditNotes.Add("Approved");

IMyCloneable<AnnualFinancialReport> prototype = original;
AnnualFinancialReport typedCopy = prototype.Clone();
var standardCopy = (AnnualFinancialReport)((ICloneable)original).Clone();

typedCopy.Title = "Next year's plan";
typedCopy.Year++;
typedCopy.Tags.Add("plan");
typedCopy.Sections.Add("Forecast");
typedCopy.Amounts["Revenue"] = 2_000_000m;
typedCopy.AuditNotes.Clear();

Print("Original", original);
Print("IMyCloneable<T> copy (modified)", typedCopy);
Print("ICloneable copy", standardCopy);

static void Print(string label, AnnualFinancialReport report)
{
    Console.WriteLine($"{label}: {report.Title}, {report.Year}, {report.Author}");
    Console.WriteLine($"  Tags: {string.Join(", ", report.Tags)}");
    Console.WriteLine($"  Sections: {string.Join(", ", report.Sections)}");
    Console.WriteLine($"  Revenue: {report.Amounts["Revenue"]} {report.Currency}");
    Console.WriteLine($"  Audit notes: {report.AuditNotes.Count}");
}
