using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public partial class CalendarHeaderWrapper : ComponentBase
{
    private string[] _abbreviatedDays { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
    private List<string> _weekDays { get; set; } = new();

    //TODO: set user preference for start of the week in the settings page
    private DayOfWeek _firstDayOfWeek { get; set; } = DayOfWeek.Monday;

    public CalendarHeaderWrapper()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        for (int i = 0; i < _abbreviatedDays.Length; i++)
        {
            int dayIndex = (i + (int)_firstDayOfWeek) % 7;
            _weekDays.Add(_abbreviatedDays[dayIndex]);
        }

        await Task.CompletedTask;
    }
}
