using MainApp.Settings.Enum;

namespace MainApp.Components.Chart;

public class ChartConfig
{
    public string Type { get; set; } = ChartType.Bar.ToString().ToLower();
    public ChartConfigData Data { get; set; } = new();
    public object Options { get; set; } = new();
}
