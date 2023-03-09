using DateTimeLibrary;
using MainApp.Settings.Enum;
using MainApp.Settings.Theme;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown;

public partial class DropdownDateRange : ComponentBase
{
    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public DateTimeRange DateTimeRange { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Theme IconColor { get; set; } = Theme.Success;

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public Theme DropdownBackground { get; set; } = Theme.Primary;

    [Parameter]
    public string DropdownLabel { get; set; } = Label.NoDateAssigned;

    private bool _isValidDateRange { get; set; } = true;

    public DropdownDateRange()
	{
        DateTimeRange = new DateTimeRange
        {
            Start = DateTime.Now,
            End = DateTime.Now
        };        
    }

    protected override async Task OnInitializedAsync()
    {
        DropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(DateTimeRange);
        await Task.CompletedTask;
    }

    private async Task ChangeDateAsync()
    {
        var isValidRange = _dateTimeService.CheckDateRange(DateTimeRange);

        if (!isValidRange)
        {
            _isValidDateRange = false;
        }
        else
        {
            _isValidDateRange = true;
            DropdownLabel = await _dropdownDateRangeService.UpdateDropdownLabel(DateTimeRange);
            await OnSubmitSuccess.InvokeAsync();
        }

        await Task.CompletedTask;
    }
}
