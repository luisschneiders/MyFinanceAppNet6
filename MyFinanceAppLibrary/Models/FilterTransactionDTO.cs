namespace MyFinanceAppLibrary.Models;

public class FilterTransactionDTO
{
#nullable disable
    public DateTimeRange DateTimeRange { get; set; }
    public ulong FromBank { get; set; } = 0;
    public ulong TCategoryId { get; set; } = 0;
    public bool IsFilterChanged { get; set; } = false;
#nullable enable
}
