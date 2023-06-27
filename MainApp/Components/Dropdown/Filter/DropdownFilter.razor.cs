using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.Filter;

public partial class DropdownFilter : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

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
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public Theme IconStartColor { get; set; } = Theme.Light;

    [Parameter]
    public string IconEnd { get; set; } = "bi-chevron-down";

    [Parameter]
    public string DropdownLabel { get; set; } = Label.NoFilterAssigned;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public FilterModel Model { get; set; } = default!;

    private string _buttonRadius { get; set; } = Radius.Default;

    public DropdownFilter()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _buttonRadius = await _appSettingsService.GetButtonShape();
            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    private async Task ResetFilter()
    {
        await OnSubmitSuccess.InvokeAsync();
        await Task.CompletedTask;
    }
}
