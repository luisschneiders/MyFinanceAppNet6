namespace DateTimeLibrary.Services;

public interface IDateTimeService
{
    DateTimeRange GetCurrentWeek();
    DateTimeRange GetCurrentMonth();
    DateTimeRange GetPreviousMonth(DateTimeRange date);
    DateTimeRange GetNextMonth(DateTimeRange date);
    DateTimeRange GetCurrentYear();
    DateTimeOffset GetCurrentTime();
    DateTime GetLastDayOfMonth(DateTimeRange dateTimeRange);
    bool CheckIsNight(string localTime);
    bool CheckDateRange(DateTimeRange dateTimeRange);
    DayOfWeek MapDayOfWeekStringToEnum(string weekday);
}
