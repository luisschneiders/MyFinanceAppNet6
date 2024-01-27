namespace MyFinanceAppLibrary.Models;

public class FilterTransactionDTO
{
#nullable disable
    public ulong FromBank { get; set; } = 0;
    public ulong TCategoryId { get; set; } = 0;

#nullable enable
}
