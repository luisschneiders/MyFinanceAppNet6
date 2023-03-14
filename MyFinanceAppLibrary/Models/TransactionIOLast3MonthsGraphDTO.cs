namespace MyFinanceAppLibrary.Models;

public class TransactionIOLast3MonthsGraphDTO
{
#nullable disable
    public int MonthNumber { get; }
    public int YearNumber { get; }
    public string Label { get; }
    public decimal TotalAmount { get; }
#nullable enable
}
