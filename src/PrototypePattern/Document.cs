namespace PrototypePattern;

public class Document : IMyCloneable<Document>, ICloneable
{
    public string Title { get; set; }
    public List<string> Tags { get; } = [];

    public Document(string title)
    {
        Title = title;
    }

    protected Document(Document source)
    {
        ArgumentNullException.ThrowIfNull(source);
        Title = source.Title;
        Tags = new List<string>(source.Tags);
    }

    public virtual Document Clone() => new(this);

    // Virtual dispatch preserves descendants' runtime types for ICloneable too.
    object ICloneable.Clone() => Clone();
}
