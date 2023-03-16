namespace MainApp.Components.Dropdown;

public interface IDropdownDateRangeService
{
    Task<string> UpdateLabel(DateTimeRange dateTimeRange);
}
