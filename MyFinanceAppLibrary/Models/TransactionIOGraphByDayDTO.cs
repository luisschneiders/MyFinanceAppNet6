namespace MyFinanceAppLibrary.Models;

public class TransactionIOGraphByDayDTO
{
#nullable disable
    public int DayNumber { get; }
    public string Label { get; }
    public decimal TotalAmount { get; }
#nullable enable
}
