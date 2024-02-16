namespace MyFinanceAppLibrary.Models;

public class TransactionIOGraphByMonthDTO
{
#nullable disable
    public int MonthNumber { get; }
    public string Label { get; }
    public decimal TotalAmount { get; }
#nullable enable
}
