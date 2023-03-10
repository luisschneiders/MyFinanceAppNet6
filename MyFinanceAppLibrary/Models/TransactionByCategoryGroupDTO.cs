namespace MyFinanceAppLibrary.Models;

public class TransactionByCategoryGroupDTO
{
#nullable disable
    public string Description { get; set; }
    public decimal Total { get; set; }
    public List<TransactionListDTO> Transactions { get; set; } = new();
#nullable enable
}
