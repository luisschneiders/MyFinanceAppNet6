namespace MainApp.Components.CalendarView;

public class CalendarViewService : ICalendarViewService
{
    public CalendarViewService()
    {
    }

    public Task<int[][]> Build(DateTimeRange dateTimeRange)
    {
        try
        {
            var firstDayOfMonth = dateTimeRange.Start;
            var lastDayOfMonth = dateTimeRange.End;
            var daysInMonth = lastDayOfMonth.Day;


            //TODO: set user preference for start of the week in the settings page
            //var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek - (int)DayOfWeek.Sunday; // Sundays
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek - (int)DayOfWeek.Monday; // Mondays

            int weeksInMonth = (int)Math.Ceiling((firstDayOfWeek + daysInMonth) / 7.0);

            int[][] weeks = new int[weeksInMonth][];

            int dayNumber = 1;

            for (int i = 0; i < weeks.Length; i++)
            {
                weeks[i] = new int[7];

                for (int j = 0; j < weeks[i].Length; j++)
                {
                    if (i == 0 && j < firstDayOfWeek)
                    {
                        weeks[i][j] = 0;
                    }
                    else if (dayNumber <= daysInMonth)
                    {
                        weeks[i][j] = dayNumber;
                        dayNumber++;
                    }
                    else
                    {
                        weeks[i][j] = 0;
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
