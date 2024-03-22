namespace MyFinanceAppLibrary.Models;

public class ExpenseListGroupByMonthDTO
{
#nullable disable
    public ulong ECategoryId { get; }
    public string ECategoryDescription { get; }
    public decimal TotalAmount { get; }
    public int MonthNumber { get; }
#nullable enable
}
