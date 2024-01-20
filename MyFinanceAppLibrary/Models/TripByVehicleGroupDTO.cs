namespace MyFinanceAppLibrary.Models;

public class TripByVehicleGroupDTO
{
#nullable disable
    public string Description { get; set; }
    public decimal Total { get; set; }
    public List<TripListDTO> Trips { get; set; } = new();
#nullable enable
}
