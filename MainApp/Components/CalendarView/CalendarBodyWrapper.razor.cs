using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.CalendarView;

public partial class CalendarBodyWrapper : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public CalendarBodyWrapper()
    {
    }
}
