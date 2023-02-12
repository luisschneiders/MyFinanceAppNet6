namespace DateTimeLibrary.Services;

public interface IDateTimeService
{
	DateTimeRangeModel GetCurrentMonth();
	bool CheckDateRange(DateTimeRangeModel dateTimeRangeModel);
}
