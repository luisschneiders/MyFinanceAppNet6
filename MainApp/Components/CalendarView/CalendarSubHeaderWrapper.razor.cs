using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public partial class CalendarSubHeaderWrapper : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public CalendarSubHeaderWrapper()
    {
    }
}
