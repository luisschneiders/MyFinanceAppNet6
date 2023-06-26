using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public class CalendarViewService : ICalendarViewService
{
    //TODO: replace ILocalStorageService with IAppSettingsService
    private ILocalStorageService _localStorageService { get; set; } = default!;

    private IDateTimeService _dateTimeService { get; set; } = default!;

    public CalendarViewService(ILocalStorageService localStorageService, IDateTimeService dateTimeService)
    {
        _localStorageService = localStorageService;
        _dateTimeService = dateTimeService;
    }

    public async Task<int[][]> Build(DateTimeRange dateTimeRange)
    {
        try
        {
            var firstDayOfMonth = dateTimeRange.Start;
            var lastDayOfMonth = dateTimeRange.End;
            var daysInMonth = lastDayOfMonth.Day;

            string appStartOfWeek = await _localStorageService.GetAsync<string>(LocalStorage.AppStartOfWeek);
            DayOfWeek selectedDayOfWeek = _dateTimeService.MapDayOfWeekStringToEnum(appStartOfWeek!);

            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek - (int)selectedDayOfWeek;

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

            return await Task.FromResult(weeks);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
