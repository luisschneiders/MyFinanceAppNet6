using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.Actions;

public partial class DropdownActions : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public string IconStart { get; set; } = string.Empty;

    [Parameter]
    public Theme IconStartColor { get; set; } = Theme.Light;

    [Parameter]
    public string IconEnd { get; set; } = "bi-three-dots";

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public string ButtonCssClass { get; set; } = string.Empty;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public string ButtonInnerStyles { get; set; } = string.Empty;

    public DropdownActions()
    {
    }
}
