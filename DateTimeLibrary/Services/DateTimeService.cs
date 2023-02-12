namespace DateTimeLibrary.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeService()
	{
	}

    public DateTimeRangeModel GetCurrentMonth()
    {
        DateTimeRangeModel dateTimeRangeModel = new();
        DateTime now = DateTime.Now;

        var startDate = new DateTime(now.Year, now.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        dateTimeRangeModel.Start = startDate;
        dateTimeRangeModel.End = endDate;

        return dateTimeRangeModel;
    }

    public bool CheckDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        return dateTimeRangeModel.Start.Date <= dateTimeRangeModel.End.Date;
    }
}
