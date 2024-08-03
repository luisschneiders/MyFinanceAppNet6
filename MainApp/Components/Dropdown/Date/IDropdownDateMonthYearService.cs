namespace MainApp.Components.Dropdown.Date;

public interface IDropdownDateMonthYearService
{
    public Task<string> UpdateLabel(DateTimeRange dateTimeRange);
}
