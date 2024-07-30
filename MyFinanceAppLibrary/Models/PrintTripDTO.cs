namespace MyFinanceAppLibrary.Models;

public class PrintTripDTO
{
    public DateTimeRange DateTimeRange { get; set; } = new();
    public List<TripByVehicleGroupDTO> TripsByGroup { get; set; } = new();
    public decimal SumByDateRange { get; set; } = 0;
}
