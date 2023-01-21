using Microsoft.AspNetCore.Components;

namespace MainApp.Components.ListGroup;

public partial class ListGroup : ComponentBase
{

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Styles { get; set; } = string.Empty;

    public ListGroup()
    {
    }
}
