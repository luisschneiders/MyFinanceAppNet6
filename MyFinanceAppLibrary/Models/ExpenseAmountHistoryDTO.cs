namespace MyFinanceAppLibrary.Models;

public class ExpenseAmountHistoryDTO
{
#nullable disable
    public ulong ECategoryId { get; }
    public string Description { get; }
    public decimal Total { get; }
    public decimal Highest { get; }
    public decimal Lowest { get; }
#nullable enable
}
