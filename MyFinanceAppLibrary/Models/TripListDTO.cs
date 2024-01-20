﻿namespace MyFinanceAppLibrary.Models;

public class TripListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }
    public DateTime TDate { get; }
    public ulong VehicleId { get; set; }
    public decimal Distance { get; }
    public decimal StartOdometer { get; }
    public decimal EndOdometer { get; }
    public int PayStatus { get; set; }
    public ulong TCategoryId { get; }
    public string VehicleDescription { get; }
    public string VehicleYear { get; }
    public string VehiclePlate { get; }
#nullable enable
}
