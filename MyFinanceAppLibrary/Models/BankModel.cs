namespace MyFinanceAppLibrary.Models;

public class BankModel : BaseModel
{
#nullable disable
    public int Id { get; set; }
    public string Account { get; set; }
    public string Description { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
#nullable enable
}
