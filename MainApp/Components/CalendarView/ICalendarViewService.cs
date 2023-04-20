namespace MainApp.Components.CalendarView;

public interface ICalendarViewService
{
    Task<string[][]> Build(DateTimeRange dateTimeRange);
}
