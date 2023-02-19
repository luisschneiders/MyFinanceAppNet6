namespace MyFinanceAppLibrary.Models;

public class TransactionModelByCategoryGroupDTO
{
#nullable disable
    public string Description { get; set; }
    public decimal Total { get; set; }
    public List<TransactionModelListDTO> Transactions { get; set; } = new();
#nullable enable
}
