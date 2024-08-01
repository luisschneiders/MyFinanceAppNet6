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
    public Theme ButtonColor { get; set; } = Theme.Dark;

    [Parameter]
    public Theme IconStartColor { get; set; } = Theme.Dark;

    [Parameter]
    public string IconEnd { get; set; } = "bi-chevron-down";

    [Parameter]
    public string DropdownLabel { get; set; } = Label.NoFilterAssigned;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    public DropdownMultiSelect()
    {
    }
}
