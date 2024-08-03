namespace MyFinanceAppLibrary.Models;

public class MultiFilterExpenseDTO
{
    public DateTimeRange DateTimeRange { get; set; } = new();
    public List<ulong> BankId { get; set; } = new();
    public List<ulong> ECategoryId { get; set; } = new();
    public bool IsFilterChanged { get; set; } = false;
}
