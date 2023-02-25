namespace MyFinanceAppLibrary.Models;

public class TransactionModelListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }
    public ulong Link { get; }
    public DateTime TDate { get; }
    public string BankDescription { get; }
    public ulong TCategoryId { get; }
    public string TCategoryDescription { get; }
    public string Action { get; }
    public string Label { get; }
    public string Comments { get; }
    public decimal Amount { get; }
#nullable enable
}
