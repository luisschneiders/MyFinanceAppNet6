using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public partial class CalendarWrapper : ComponentBase
{
    [Parameter]
    public RenderFragment? CalendarSubHeaderView { get; set; }

    [Parameter]
    public RenderFragment? CalendarHeaderView { get; set; }

    [Parameter]
    public RenderFragment? CalendarBodyView { get; set; }

    [Parameter]
    public bool CalendarHasSubheader { get; set; } = false;

    public CalendarWrapper()
    {
    }
}
