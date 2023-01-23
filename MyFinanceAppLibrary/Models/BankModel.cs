namespace MyFinanceAppLibrary.Models;

public class BankModel
{
#nullable disable
    public int Id { get; set; }
    public string Account { get; set; }
    public string Description { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
#nullable enable
}
