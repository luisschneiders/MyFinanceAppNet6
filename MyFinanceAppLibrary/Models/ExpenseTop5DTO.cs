namespace MyFinanceAppLibrary.Models;

public class ExpenseTop5DTO
{
#nullable disable
    public ulong ECategoryId { get; }
    public string ECategoryDescription { get; }
    public string ECategoryColor { get; }
    public decimal TotalAmount { get; }
#nullable enable
}
