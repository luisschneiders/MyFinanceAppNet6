namespace MyFinanceAppLibrary.Models;

public class ExpenseCalendarDTO
{
#nullable disable
    public DateTime EDate { get; set; }
    public string ECategoryDescription { get; set; }
    public string ECategoryColor { get; set; }
    public decimal Amount { get; set; }
#nullable enable
}
