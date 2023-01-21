namespace MainApp.Components.Chart;

	public class ChartConfigData
{
    public List<string> Labels { get; set; } = new();
    public List<ChartConfigDataset> Datasets { get; set; } = new();

    public ChartConfigData()
    {
    }
}
