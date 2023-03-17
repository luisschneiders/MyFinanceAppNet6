namespace MainApp.Components.Dropdown.DateRange;

public class DropdownDateRangeService : IDropdownDateRangeService
{
    public DropdownDateRangeService()
    {
    }

    public async Task<string> UpdateLabel(DateTimeRange dateTimeRange)
    {
        string label = string.Empty;

        if (dateTimeRange.Start.Date == dateTimeRange.End.Date)
        {
            label = $"{dateTimeRange.Start.Date.ToString("dd/MM/yy")}";
        }
        else
        {
            label = $"{dateTimeRange.Start.Date.ToString("dd/MM/yy")} - {dateTimeRange.End.Date.ToString("dd/MM/yy")}";
        }
        return await Task.FromResult(label);
    }
}
