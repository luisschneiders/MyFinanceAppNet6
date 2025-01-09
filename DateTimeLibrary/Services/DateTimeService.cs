namespace DateTimeLibrary.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeService()
    {
    }

    public DateTimeRange GetCurrentWeek()
    {
        DateTimeRange dateTimeRange = new();

        var firstDayOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
        var lastDayOfWeek = firstDayOfWeek.AddDays(6);

        dateTimeRange.Start = firstDayOfWeek;
        dateTimeRange.End = lastDayOfWeek;

        return dateTimeRange;
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

    public DateTimeRange GetPreviousMonth(DateTimeRange date)
    {
        DateTimeRange dateTimeRange = new()
        {
            Start = date.Start.AddMonths(-1),
            End = date.End.AddMonths(-1)
        };

        dateTimeRange.End = GetLastDayOfMonth(dateTimeRange);

        return dateTimeRange;
    }

    public DateTimeRange GetNextMonth(DateTimeRange date)
    {
        DateTimeRange dateTimeRange = new()
        {
            Start = date.Start.AddMonths(1),
            End = date.End.AddMonths(1)
        };

        dateTimeRange.End = GetLastDayOfMonth(dateTimeRange);

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

    public DateTime GetLastDayOfMonth(DateTimeRange dateTimeRange)
    {
        DateTime lastDay = dateTimeRange.Start.AddMonths(1).AddDays(-1);
        return lastDay;
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

    public DayOfWeek MapDayOfWeekStringToEnum(string weekday)
    {
        var dict = new Dictionary<string, DayOfWeek>
        {
            { "Sunday", DayOfWeek.Sunday },
            { "Monday", DayOfWeek.Monday },
            { "Tuesday", DayOfWeek.Tuesday },
            { "Wednesday", DayOfWeek.Wednesday },
            { "Thursday", DayOfWeek.Thursday },
            { "Friday", DayOfWeek.Friday },
            { "Saturday", DayOfWeek.Saturday },
        };

        var firstDayOfWeek = dict[weekday];

        return firstDayOfWeek;
    }
}
