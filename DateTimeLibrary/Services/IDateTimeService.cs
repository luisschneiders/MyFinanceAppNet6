namespace DateTimeLibrary.Services;

public interface IDateTimeService
{
	DateTimeRange GetCurrentMonth();
	DateTimeRange GetCurrentYear();
    DateTimeOffset GetCurrentTime();
    DateTime GetLastDayOfMonth(DateTimeRange dateTimeRange);
    bool CheckIsNight(string localTime);
    bool CheckDateRange(DateTimeRange dateTimeRange);
}
