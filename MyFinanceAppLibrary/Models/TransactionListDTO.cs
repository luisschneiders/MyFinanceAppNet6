namespace MyFinanceAppLibrary.Models;

public class TransactionListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }
    public ulong Link { get; }
    public DateTime TDate { get; }
    public ulong FromBank { get; }
    public string BankDescription { get; }
    public ulong TCategoryId { get; set; }
    public string TCategoryDescription { get; }
    public string TCategoryColor {get;}
    public string Action { get; }
    public string Label { get; }
    public string Comments { get; }
    public decimal Amount { get; }
#nullable enable
}
