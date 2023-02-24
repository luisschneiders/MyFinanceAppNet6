namespace MyFinanceAppLibrary.Models;

public class TripModelListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }
    public DateTime TDate { get; }
    public decimal Distance { get; }
    public string VehicleDescription { get; }
    public string VehiclePlate { get; }
#nullable enable
}
