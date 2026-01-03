namespace MyFinanceAppLibrary.Models;

public class BankTransactionHistoryListDTO
{
#nullable disable
    public DateTime BDate { get; }
    public string BankDescription { get; }
    public string ActionDescription { get; }
    public decimal PreviousBalance { get; }
    public decimal Amount { get; }
    public decimal CurrentBalance { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
#nullable enable
}
