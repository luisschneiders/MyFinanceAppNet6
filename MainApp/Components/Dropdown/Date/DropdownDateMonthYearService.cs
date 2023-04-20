namespace MainApp.Components.Dropdown.Date;

public class DropdownDateMonthYearService : IDropdownDateMonthYearService
{
    public DropdownDateMonthYearService()
    {
    }

    public async Task<string> UpdateLabel(DateTimeRange dateTimeRange)
    {
        string label = $"{dateTimeRange.Start.Date.ToString("MM/yyyy")}";
        return await Task.FromResult(label);
    }
}

