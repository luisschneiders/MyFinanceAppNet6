namespace MyFinanceAppLibrary.Models;

public class MultiFilterExpenseByMonthDTO
{
    public DateTimeRange DateTimeRange { get; set; } = new();
    public List<ulong> ECategoryId { get; set; } = new();
    public bool IsFilterChanged { get; set; } = false;

}
