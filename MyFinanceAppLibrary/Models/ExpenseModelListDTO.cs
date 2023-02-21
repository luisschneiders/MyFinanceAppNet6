namespace MyFinanceAppLibrary.Models;

public class ExpenseModelListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }
    public DateTime EDate { get; }
    public string BankDescription { get; }
    public string ExpenseCategoryDescription { get; }
    public string Comments { get; }
    public decimal Amount { get; }
#nullable enable
}
