namespace MyFinanceAppLibrary.Models;

public class MultiFilterTripDTO
{
    public DateTimeRange DateTimeRange { get; set; } = new();
    public List<ulong> VehicleId { get; set; } = new();
    public List<ulong> TCategoryId { get; set; } = new();
    public bool IsFilterChanged { get; set; } = false;
}
