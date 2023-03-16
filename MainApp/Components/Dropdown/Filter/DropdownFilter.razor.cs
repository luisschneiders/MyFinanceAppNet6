﻿using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.Filter;

public partial class DropdownFilter : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public string IconStart { get; set; } = string.Empty;

    [Parameter]
    public Theme IconStartColor { get; set; } = Theme.Light;

    [Parameter]
    public string IconEnd { get; set; } = "bi-chevron-down";

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public string DropdownLabel { get; set; } = Label.NoFilterAssigned;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public Theme DropdownBackground { get; set; } = Theme.Primary;

    [Parameter]
    public FilterModel Model { get; set; } = default!;

    public DropdownFilter()
    {
    }

    private async Task ResetFilter()
    {
        await OnSubmitSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
