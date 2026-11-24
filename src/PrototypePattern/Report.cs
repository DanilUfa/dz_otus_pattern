namespace PrototypePattern;

public class Report : Document, IMyCloneable<Report>
{
    public string Author { get; set; }
    public List<string> Sections { get; } = [];

    public Report(string title, string author) : base(title)
    {
        Author = author;
    }

    protected Report(Report source) : base(source)
    {
        Author = source.Author;
        Sections = new List<string>(source.Sections);
    }

    public override Report Clone() => new(this);

    Report IMyCloneable<Report>.Clone() => Clone();
}
