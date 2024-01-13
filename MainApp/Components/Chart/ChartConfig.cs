namespace MainApp.Components.Chart;

public class ChartConfig
{
    public string Type { get; set; } = ChartType.bar.ToString();
    public ChartConfigData Data { get; set; } = new();
    public ChartConfigOption Option { get; set; } = new();
}
