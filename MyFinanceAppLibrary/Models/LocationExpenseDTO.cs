namespace MyFinanceAppLibrary.Models;

public class LocationExpenseDTO
{
#nullable disable
    public string LocationId { get; }
    public string Address { get; }
    public decimal Latitude { get; }
    public decimal Longitude { get; }
#nullable enable
}
