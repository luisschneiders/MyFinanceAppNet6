using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.MultiSelect;

public partial class DropdownMultiSelect : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public string IconStart { get; set; } = string.Empty;

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Secondary;

    [Parameter]
    public Theme IconStartColor { get; set; } = Theme.Secondary;

    [Parameter]
    public string IconEnd { get; set; } = "bi-chevron-down";

    [Parameter]
    public string DropdownLabel { get; set; } = Label.AppNoFilterAssigned;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public bool AutoClose { get; set; } = false;

    public DropdownMultiSelect()
    {
    }
}
