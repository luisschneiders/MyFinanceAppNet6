namespace MyFinanceAppLibrary.Models;

public class LocationModel
{
#nullable disable
    public string Id { get; set; }
    public string Address { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
#nullable enable
}
