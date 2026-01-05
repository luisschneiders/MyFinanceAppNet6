namespace MyFinanceAppLibrary.Models;

public class MultiFilterBankTransactionHistoryDTO
{
    public DateTimeRange? DateTimeRange { get; set; } = new();
    public ulong BankId { get; set; }
    public List<string> Action { get; set; } = new();
    public bool IsFilterChanged { get; set; } = false;
    public LoadMore LoadMore { get; set; }
}
