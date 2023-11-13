namespace MyFinanceAppLibrary.Models;

public class ExpenseListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }
    public DateTime EDate { get; }
    public string BankDescription { get; }
    public ulong ECategoryId { get; set; }
    public string ExpenseCategoryDescription { get; }
    public string ExpenseCategoryColor { get; }
    public string Comments { get; }
    public decimal Amount { get; }
#nullable enable
}
