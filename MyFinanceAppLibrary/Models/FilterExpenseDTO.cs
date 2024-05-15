namespace MyFinanceAppLibrary.Models;

public class FilterExpenseDTO
{
#nullable disable
    public DateTimeRange DateTimeRange { get; set; }
    public ulong BankId { get; set; } = 0;
    public ulong ECategoryId { get; set; } = 0;
    public bool IsFilterChanged { get; set; } = false;
#nullable enable
}
