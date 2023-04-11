namespace DateTimeLibrary.Services;

public interface IDateTimeService
{
	DateTimeRange GetCurrentMonth();
	DateTimeRange GetCurrentYear();
	DateTimeOffset GetCurrentTime();
	bool CheckDateRange(DateTimeRange dateTimeRange);
}
