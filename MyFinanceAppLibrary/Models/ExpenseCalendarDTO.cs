namespace MyFinanceAppLibrary.Models;

public class ExpenseCalendarDTO
{
#nullable disable
    public DateTime EDate { get; set; }
    public string ExpenseCategoryDescription { get; set; }
    public string ExpenseCategoryColor { get; set; }
    public decimal Amount { get; set; }
#nullable enable
}
