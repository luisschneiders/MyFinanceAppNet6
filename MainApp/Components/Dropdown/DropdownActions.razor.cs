using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown;

public partial class DropdownActions : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public string Icon { get; set; } = "bi-three-dots";

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    public DropdownActions()
    {
    }
}
