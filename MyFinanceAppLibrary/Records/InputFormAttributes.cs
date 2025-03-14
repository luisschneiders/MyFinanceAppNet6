namespace MyFinanceAppLibrary.Records;

public record class InputFormAttributes
{
    public Dictionary<string, object> Control = default!;
    public Dictionary<string, object> Select = default!;
    public Dictionary<string, object> PlainText = default!;
    public Dictionary<string, object> Disabled = default!;
}
