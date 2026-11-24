using PrototypePattern;

// A dependency-free test runner. Failed checks cause exit code 1.
var tests = new (string Name, Action Run)[]
{
    ("Document: both interfaces", () => VerifyBoth(new Document("Document"))),
    ("Report: both interfaces", () => VerifyBoth(new Report("Report", "Author"))),
    ("FinancialReport: both interfaces", () => VerifyBoth(new FinancialReport("Finance", "Author", "RUB"))),
    ("AnnualFinancialReport: both interfaces", () => VerifyBoth(CreateAnnual())),
    ("Typed interfaces on every level", VerifyTypedInterfaces),
    ("Polymorphic clone through base references", VerifyPolymorphism),
    ("Copies do not share mutable state", VerifyIndependentCopies)
};
var failures = 0;
foreach (var (name, run) in tests)
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
Console.WriteLine($"{tests.Length - failures}/{tests.Length} checks passed.");
return failures == 0 ? 0 : 1;

static AnnualFinancialReport CreateAnnual() => new("Annual", "Author", "RUB", 2026);

static void VerifyBoth(Document original)
{
    original.Tags.Add("original tag");
    if (original is Report report) report.Sections.Add("original section");
    if (original is FinancialReport finance) finance.Amounts.Add("Revenue", 100m);
    if (original is AnnualFinancialReport annual) annual.AuditNotes.Add("original note");
    VerifyCopy(original, ((IMyCloneable<Document>)original).Clone());
    VerifyCopy(original, (Document)((ICloneable)original).Clone());
}

static void VerifyCopy(Document original, Document copy)
{
    Check(!ReferenceEquals(original, copy), "Clone is the original instance.");
    Check(original.GetType() == copy.GetType(), "Runtime type was lost.");
    Check(original.Title == copy.Title && original.Tags.SequenceEqual(copy.Tags), "Document state differs.");
    Check(!ReferenceEquals(original.Tags, copy.Tags), "Tags are shared.");
    copy.Title = "changed";
    copy.Tags.Add("changed");
    Check(original.Title != copy.Title && original.Tags.Count == 1, "Original document changed.");

    if (original is Report report && copy is Report reportCopy)
    {
        Check(report.Author == reportCopy.Author && report.Sections.SequenceEqual(reportCopy.Sections), "Report state differs.");
        Check(!ReferenceEquals(report.Sections, reportCopy.Sections), "Sections are shared.");
        reportCopy.Author = "changed";
        reportCopy.Sections.Clear();
        Check(report.Author == "Author" && report.Sections.Count == 1, "Original report changed.");
    }
    if (original is FinancialReport finance && copy is FinancialReport financeCopy)
    {
        Check(finance.Currency == financeCopy.Currency && finance.Amounts.SequenceEqual(financeCopy.Amounts), "Financial state differs.");
        Check(!ReferenceEquals(finance.Amounts, financeCopy.Amounts), "Amounts are shared.");
        financeCopy.Currency = "USD";
        financeCopy.Amounts["Revenue"] = 200m;
        Check(finance.Currency == "RUB" && finance.Amounts["Revenue"] == 100m, "Original financial report changed.");
    }
    if (original is AnnualFinancialReport annual && copy is AnnualFinancialReport annualCopy)
    {
        Check(annual.Year == annualCopy.Year && annual.AuditNotes.SequenceEqual(annualCopy.AuditNotes), "Annual state differs.");
        Check(!ReferenceEquals(annual.AuditNotes, annualCopy.AuditNotes), "Audit notes are shared.");
        annualCopy.Year++;
        annualCopy.AuditNotes.Clear();
        Check(annual.Year == 2026 && annual.AuditNotes.Count == 1, "Original annual report changed.");
    }
}

static void VerifyTypedInterfaces()
{
    Document document = ((IMyCloneable<Document>)new Document("D")).Clone();
    Report report = ((IMyCloneable<Report>)new Report("R", "A")).Clone();
    FinancialReport finance = ((IMyCloneable<FinancialReport>)new FinancialReport("F", "A", "RUB")).Clone();
    AnnualFinancialReport annual = ((IMyCloneable<AnnualFinancialReport>)CreateAnnual()).Clone();
    Check(document.GetType() == typeof(Document) && report.GetType() == typeof(Report)
        && finance.GetType() == typeof(FinancialReport) && annual.GetType() == typeof(AnnualFinancialReport),
        "Typed interface returns an unexpected type.");
}

static void VerifyPolymorphism()
{
    var annual = CreateAnnual();
    Document asDocument = annual;
    Report asReport = annual;
    FinancialReport asFinance = annual;
    Document[] copies = [asDocument.Clone(), asReport.Clone(), asFinance.Clone(),
        ((IMyCloneable<Document>)annual).Clone(), ((IMyCloneable<Report>)annual).Clone(),
        ((IMyCloneable<FinancialReport>)annual).Clone(), (Document)((ICloneable)annual).Clone()];
    Check(copies.All(copy => copy is AnnualFinancialReport && !ReferenceEquals(copy, annual)),
        "Clone through a base reference lost its runtime type.");
}

static void VerifyIndependentCopies()
{
    var original = CreateAnnual();
    original.Tags.Add("source");
    original.Sections.Add("source");
    original.Amounts.Add("Revenue", 100m);
    original.AuditNotes.Add("source");
    var first = original.Clone();
    var second = original.Clone();
    original.Tags.Clear();
    original.Sections.Clear();
    original.Amounts.Clear();
    original.AuditNotes.Clear();
    first.Tags.Clear();
    first.Sections.Clear();
    first.Amounts.Clear();
    first.AuditNotes.Clear();
    Check(second.Tags.Count == 1 && second.Sections.Count == 1
        && second.Amounts["Revenue"] == 100m && second.AuditNotes.Count == 1,
        "A copy shares mutable state with the original or a sibling copy.");
}

static void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}
