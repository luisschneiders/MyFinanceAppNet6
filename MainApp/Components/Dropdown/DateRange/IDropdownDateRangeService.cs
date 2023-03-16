namespace MainApp.Components.Dropdown.DateRange;

public interface IDropdownDateRangeService
{
    Task<string> UpdateLabel(DateTimeRange dateTimeRange);
}
