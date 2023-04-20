namespace MainApp.Components.Dropdown.Date;

public interface IDropdownDateMonthYearService
{
    Task<string> UpdateLabel(DateTimeRange dateTimeRange);
}
