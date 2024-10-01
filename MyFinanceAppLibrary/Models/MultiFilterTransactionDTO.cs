namespace MyFinanceAppLibrary.Models;

public class MultiFilterTransactionDTO
{
    public DateTimeRange DateTimeRange { get; set; } = new();
    public List<ulong> FromBank { get; set; } = new();
    public List<ulong> TCategoryId { get; set; } = new();
    public bool IsFilterChanged { get; set; } = false;
}
