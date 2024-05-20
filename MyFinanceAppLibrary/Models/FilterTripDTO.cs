namespace MyFinanceAppLibrary.Models;

public class FilterTripDTO
{
#nullable disable
    public DateTimeRange DateTimeRange { get; set; }
    public ulong VehicleId { get; set; } = 0;
    public ulong TCategoryId { get; set; }
    public bool IsFilterChanged { get; set; } = false;
#nullable enable
}
