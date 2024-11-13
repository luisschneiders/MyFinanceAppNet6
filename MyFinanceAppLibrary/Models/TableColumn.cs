namespace MyFinanceAppLibrary.Models;

public class TableColumn
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
    public bool IsDisabled { get; set; }
    public string CssClass { get; set; } = string.Empty;
    public string Colspan { get; set; } = string.Empty;
}
