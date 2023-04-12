namespace DateTimeLibrary.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeService()
	{
	}

    public DateTimeRange GetCurrentMonth()
    {
        DateTimeRange dateTimeRange = new();
        DateTime now = DateTime.Now;

        var startDate = new DateTime(now.Year, now.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        dateTimeRange.Start = startDate;
        dateTimeRange.End = endDate;

        return dateTimeRange;
    }

    public DateTimeRange GetCurrentYear()
    {
        DateTimeRange dateTimeRange = new();
        DateTime now = DateTime.Now;

        var startDate = new DateTime(now.Year, 1, 1);
        var endDate = new DateTime(now.Year, 12, 31);

        dateTimeRange.Start = startDate;
        dateTimeRange.End = endDate;

        return dateTimeRange;
    }

    public bool CheckDateRange(DateTimeRange dateTimeRange)
    {
        return dateTimeRange.Start.Date <= dateTimeRange.End.Date;
    }

    public DateTimeOffset GetCurrentTime()
    {
        return DateTimeOffset.Now;        
    }

    public bool CheckIsNight(string localTime)
    {
        DateTime dateTime = DateTime.Parse(localTime);

        return (dateTime.Hour >= 18 || dateTime.Hour < 6);
    }
}
