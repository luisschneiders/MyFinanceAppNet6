namespace MainApp.Components.Dropdown;

public interface IDropdownDateRangeService
{
    Task<string> UpdateDropdownLabel(DateTimeRange dateTimeRange);
}
