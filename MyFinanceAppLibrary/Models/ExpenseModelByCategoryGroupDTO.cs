namespace MyFinanceAppLibrary.Models;

public class ExpenseModelByCategoryGroupDTO
{
#nullable disable
    public string Description { get; set; }
    public decimal Total { get; set; }
    public List<ExpenseModelListDTO> Expenses { get; set; } = new();
#nullable enable
}
