using MainApp.Shared;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.ListGroup;

public partial class ListGroupItem : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string ItemURL { get; set; } = string.Empty;

    [Parameter]
    public string IconStart { get; set; } = string.Empty;

    [Parameter]
    public string IconEnd { get; set; } = "bi bi-chevron-right";

    [Parameter]
    public string Title { get; set; } = "Add title";

    [Parameter]
    public string Summary { get; set; } = "Add a small summary here...";

    [Parameter]
    public bool Disabled { get; set; } = false;

    public ListGroupItem()
    {
    }
}
