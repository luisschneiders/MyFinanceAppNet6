namespace MyFinanceAppLibrary.Models;

public class LocationExpenseModel : BaseModel
{
#nullable disable
    public int Id { get; set; }
    public DateTime LDate { get; set; }
    public int ExpenseId { get; set; }
    public string LocationId { get; set; }
    public string Address { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
#nullable enable
}
