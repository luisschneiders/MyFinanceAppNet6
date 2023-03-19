namespace MyFinanceAppLibrary.Models;

public class ExpenseByCategoryGroupDTO
{
#nullable disable
    public string Description { get; set; }
    public string Color { get; set; }
    public decimal Total { get; set; }
    public List<ExpenseListDTO> Expenses { get; set; } = new();
#nullable enable
}
