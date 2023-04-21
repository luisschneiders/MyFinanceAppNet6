namespace MainApp.Components.CalendarView;

public interface ICalendarViewService
{
    Task<int[][]> Build(DateTimeRange dateTimeRange);
}
