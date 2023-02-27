namespace DateTimeLibrary.Services;

public interface IDateTimeService
{
	DateTimeRange GetCurrentMonth();
	bool CheckDateRange(DateTimeRange dateTimeRange);
}
