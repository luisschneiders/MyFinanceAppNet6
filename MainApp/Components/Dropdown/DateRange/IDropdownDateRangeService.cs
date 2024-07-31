namespace MainApp.Components.Dropdown.DateRange;

public interface IDropdownDateRangeService
{
    public Task<string> UpdateLabel(DateTimeRange dateTimeRange);
}
