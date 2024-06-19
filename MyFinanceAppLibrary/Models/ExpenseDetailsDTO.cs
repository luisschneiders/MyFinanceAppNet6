namespace MyFinanceAppLibrary.Models;

public class ExpenseDetailsDTO
{
    #nullable disable
    public ulong Id { get; }
    public DateTime EDate { get;}
    public string BankDescription { get; }
    public string ExpenseCategoryDescription { get; }
    public string ExpenseCategoryColor { get; }
    public string Comments { get; }
    public decimal Amount { get; }
    public string Address { get; }
    #nullable enable
}
