namespace MyFinanceAppLibrary.Models;

public class TransactionDetailsDTO
{
    #nullable disable
    public ulong Id { get; }
    public DateTime TDate { get;}
    public string BankDescription { get; }
    public string TCategoryDescription { get; }
    public string TCategoryColor { get; set; }
    public string Label { get; set; }
    public string Comments { get; }
    public decimal Amount { get; }
    #nullable enable
}
