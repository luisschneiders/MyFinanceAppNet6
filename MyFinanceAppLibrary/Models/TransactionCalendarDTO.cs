namespace MyFinanceAppLibrary.Models;

public class TransactionCalendarDTO
{
#nullable disable
    public DateTime TDate { get; set; }
    public string TransactionCategoryDescription { get; set; }
    public string TransactionCategoryColor { get; set; }
    public decimal Amount { get; set; }
#nullable enable
}
