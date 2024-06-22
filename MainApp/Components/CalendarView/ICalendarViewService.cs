namespace MainApp.Components.CalendarView;

public interface ICalendarViewService
{
    Task<DateTime[][]> Build(DateTimeRange dateTimeRange);
}
