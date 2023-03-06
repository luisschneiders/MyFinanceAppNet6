namespace DateTimeLibrary.Services;

public interface IDateTimeService
{
	DateTimeRange GetCurrentMonth();
	DateTimeRange GetCurrentYear();
	bool CheckDateRange(DateTimeRange dateTimeRange);
}
