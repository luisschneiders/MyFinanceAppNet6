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
            DateTime firstDayOfMonth = dateTimeRange.Start;
            DateTime lastDayOfMonth = dateTimeRange.End;
            int daysInMonth = lastDayOfMonth.Day;
            int weeksInMonth = 6; // maximum number of weeks in any given month

            string appStartOfWeek = await _localStorageService.GetAsync<string>(LocalStorage.AppStartOfWeek);
            DayOfWeek selectedDayOfWeek = _dateTimeService.MapDayOfWeekStringToEnum(appStartOfWeek!);

            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            int[][] weeks = new int[weeksInMonth][];

            int dayNumber = 1;

            for (int i = 0; i < weeks.Length; i++)
            {
                weeks[i] = new int[7];

                for (int j = 0; j < weeks[i].Length; j++)
                {
                    // Week starting on Monday
                    if (selectedDayOfWeek == DayOfWeek.Monday)
                    {
                        if (firstDayOfWeek == 0)
                        {
                            if (i == 0 && j < 6)
                            {
                                weeks[i][j] = 0;
                            }
                            else if (dayNumber <= daysInMonth)
                            {
                                weeks[i][j] = dayNumber;
                                dayNumber++;
                            }
                        }
                        else
                        {
                            if (i == 0 && j < (firstDayOfWeek - 1))
                            {
                                weeks[i][j] = 0;
                            }
                            else if (dayNumber <= daysInMonth)
                            {
                                weeks[i][j] = dayNumber;
                                dayNumber++;
                            }
                        }
                    }

                    // Week starting on Sunday
                    if (selectedDayOfWeek == DayOfWeek.Sunday)
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
