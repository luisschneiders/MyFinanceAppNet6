namespace MyFinanceAppLibrary.Models;

public class WeatherModel
{
#nullable disable
    public LocationModel Location { get; set; } = new();
    public WeatherConditionModel Condition { get; set; } = new();
    public string LocalTime { get; set; }
#nullable enable
}
