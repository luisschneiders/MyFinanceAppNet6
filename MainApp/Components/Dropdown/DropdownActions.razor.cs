using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown;

public partial class DropdownActions : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; }

    [Parameter]
    public Theme IconColor { get; set; }

    public DropdownActions()
    {
        IsDisplayLargeNone = false;
        IconColor = Theme.Success;
    }
}
