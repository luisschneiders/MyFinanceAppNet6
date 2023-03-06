namespace MainApp.Components.Chart;

public class ChartConfigDataset
{
    public string Label { get; set; } = string.Empty;
    public List<string> Data { get; set; } = new();
    public List<string> BackgroundColor { get; set; } = new();
    public List<string> BorderColor { get; set; } = new();
    public int BorderWidth { get; set; } = 1;
}
