namespace MyFinanceAppLibrary.Models;

public class FilterExpenseByMonthDTO
{
    #nullable disable
    public DateTimeRange DateTimeRange { get; set; }
    public ExpenseCategoryModel ExpenseCategoryModel { get; set; }
    public bool IsFilterChanged { get; set; } = false;
    #nullable enable
}
