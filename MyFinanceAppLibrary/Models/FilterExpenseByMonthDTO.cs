namespace MyFinanceAppLibrary.Models;

public class FilterExpenseByMonthDTO
{
    #nullable disable
    public DateTimeRange DateTimeRange { get; set; }
    public ExpenseCategoryModel ExpenseCategoryModel { get; set; }
    public bool IsDateChanged { get; set; } = false;
    public bool IsFilterChanged { get; set; } = false;
    #nullable enable
}
