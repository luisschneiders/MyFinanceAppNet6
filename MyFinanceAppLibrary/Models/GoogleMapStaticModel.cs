namespace MyFinanceAppLibrary.Models;

public class GoogleMapStaticModel
{
#nullable disable
    public string Marker { get; set; }
    public string Location { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Scale { get; set; } = 2;
#nullable enable
}
