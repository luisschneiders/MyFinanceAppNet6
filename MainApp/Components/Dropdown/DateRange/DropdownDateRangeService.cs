namespace MainApp.Components.Dropdown;

public class DropdownDateRangeService : IDropdownDateRangeService
{
    public DropdownDateRangeService()
    {
    }

    public async Task<string> UpdateLabel(DateTimeRange dateTimeRange)
    {
        string dateRangeDescription = string.Empty;

        if (dateTimeRange.Start.Date == dateTimeRange.End.Date)
        {
            dateRangeDescription = $"{dateTimeRange.Start.Date.ToString("dd/MM/yy")}";
        }
        else
        {
            dateRangeDescription = $"{dateTimeRange.Start.Date.ToString("dd/MM/yy")} - {dateTimeRange.End.Date.ToString("dd/MM/yy")}";
        }
        return await Task.FromResult(dateRangeDescription);
    }
}
