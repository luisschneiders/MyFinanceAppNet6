namespace MyFinanceAppLibrary.Models;

public class CheckboxItemModel
{
    public ulong Id { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public bool IsChecked { get; set; } = false;
}
