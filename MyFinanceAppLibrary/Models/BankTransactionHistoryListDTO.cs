namespace MyFinanceAppLibrary.Models;

public class BankTransactionHistoryListDTO
{
#nullable disable
    public ulong? Id { get; set; }
    public DateTime BDate { get; }
    public string BankDescription { get; }
    public string Action { get; }
    public string ActionDescription { get; }
    public decimal PreviousBalance { get; }
    public decimal Amount { get; }
    public decimal CurrentBalance { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
#nullable enable
}
