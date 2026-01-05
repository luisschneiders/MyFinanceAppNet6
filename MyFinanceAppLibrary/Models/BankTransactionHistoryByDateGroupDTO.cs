namespace MyFinanceAppLibrary.Models;

public class BankTransactionHistoryByDateGroupDTO
{
#nullable disable
    public DateTime BDate { get; set; }
    public List<BankTransactionHistoryListDTO> Transactions { get; set; } = new();
#nullable enable
}
