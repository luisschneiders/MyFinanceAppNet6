using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public partial class CalendarHeaderWrapper : ComponentBase
{
    [Inject]
    private ICalendarViewService _calendarViewService { get; set; } = default!;
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    private string[] _abbreviatedDays { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
    private List<string> _weekDays { get; set; } = new();

    public CalendarHeaderWrapper()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        string startOfWeek = await _calendarViewService.GetLocalStorageStartOfWeek();
        DayOfWeek selectedDayOfWeek = _dateTimeService.MapDayOfWeekStringToEnum(startOfWeek);

        for (int i = 0; i < _abbreviatedDays.Length; i++)
        {
            int dayIndex = (i + (int)selectedDayOfWeek) % 7;
            _weekDays.Add(_abbreviatedDays[dayIndex]);
        }

        await Task.CompletedTask;
    }
}
