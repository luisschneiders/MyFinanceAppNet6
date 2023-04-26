using System.Globalization;
using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public partial class CalendarWrapper : ComponentBase
{
    [Parameter]
    public RenderFragment? CalendarHeaderView { get; set; }

    [Parameter]
    public RenderFragment? CalendarBodyView { get; set; }

    public CalendarWrapper()
    {
    }
}
