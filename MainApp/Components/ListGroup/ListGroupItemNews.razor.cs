using Microsoft.AspNetCore.Components;

namespace MainApp.Components.ListGroup;

public partial class ListGroupItemNews : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Url { get; set; } = string.Empty;

    [Parameter]
    public string Image { get; set; } = string.Empty;

    [Parameter]
    public string Title { get; set; } = "Add title";

    [Parameter]
    public string Summary { get; set; } = "Add a small summary here...";

    public ListGroupItemNews()
    {
    }
}
