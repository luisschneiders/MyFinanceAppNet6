namespace MyFinanceAppLibrary.Models;

public class FinnhubNewsModel
{
#nullable disable
    public int Id { get; set; }
    public string Category { get; set; }
    public DateTime Datetime { get; set; }
    public string Headline { get; set; }
    public string Image { get; set; }
    public string Source { get; set; }
    public string Summary { get; set; }
    public string Url { get; set; }
#nullable enable
}
