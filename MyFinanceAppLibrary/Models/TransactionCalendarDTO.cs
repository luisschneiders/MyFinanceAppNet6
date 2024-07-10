namespace MyFinanceAppLibrary.Models;

public class TransactionCalendarDTO
{
#nullable disable
    public DateTime TDate { get; set; }
    public string TCategoryDescription { get; set; }
    public string TCategoryColor { get; set; }
    public decimal Amount { get; set; }
#nullable enable
}
