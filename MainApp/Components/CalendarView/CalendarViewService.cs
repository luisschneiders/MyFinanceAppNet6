namespace MainApp.Components.CalendarView;

public class CalendarViewService : ICalendarViewService
{
    public CalendarViewService()
    {
    }

    public Task<string[][]> Build(DateTimeRange dateTimeRange)
    {
        try
        {
            var firstDayOfMonth = dateTimeRange.Start;
            var lastDayOfMonth = dateTimeRange.End;
            var daysInMonth = lastDayOfMonth.Day;
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            int weeksInMonth = (int)Math.Ceiling((firstDayOfWeek + daysInMonth) / 7.0);

            string[][] weeks = new string[weeksInMonth][];

            int dayNumber = 1;

            for (int i = 0; i < weeks.Length; i++)
            {
                weeks[i] = new string[7];

                for (int j = 0; j < weeks[i].Length; j++)
                {
                    if (i == 0 && j < firstDayOfWeek)
                    {
                        weeks[i][j] = "";
                    }
                    else if (dayNumber <= daysInMonth)
                    {
                        weeks[i][j] = dayNumber.ToString();
                        dayNumber++;
                    }
                    else
                    {
                        weeks[i][j] = "";
                    }
                }
            }

            return Task.FromResult(weeks);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
