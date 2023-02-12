using DateTimeLibrary;
using MainApp.Settings.Enum;
using MainApp.Settings.Theme;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown;

public partial class DropdownDateRange : ComponentBase
{
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public DateTimeRangeModel DateTimeRange { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Theme IconColor { get; set; } = Theme.Success;

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public string DropdownPosition { get; set; } = "dropdown";

    private string _dateRangeLabel { get; set; } = "No date assigned!";

    private bool _isValidDateRange { get; set; } = true;

    public DropdownDateRange()
	{
        DateTimeRange = new DateTimeRangeModel
        {
            Start = DateTime.Now,
            End = DateTime.Now
        };        
    }

    protected override async Task OnInitializedAsync()
    {
        _dateRangeLabel = await UpdateDateRangeLabel();
        await Task.CompletedTask;
    }

    private async Task<string> UpdateDateRangeLabel()
    {
        string dateRangeDescription = string.Empty;

        if (DateTimeRange.Start.Date == DateTimeRange.End.Date)
        {
            dateRangeDescription = $"{DateTimeRange.Start.Date.ToString("dd/MM/yy")}";
        }
        else
        {
            dateRangeDescription = $"{DateTimeRange.Start.Date.ToString("dd/MM/yy")} - {DateTimeRange.End.Date.ToString("dd/MM/yy")}";
        }

        return await Task.FromResult(dateRangeDescription);
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
            _dateRangeLabel = await UpdateDateRangeLabel();

            await OnSubmitSuccess.InvokeAsync();
        }

        await Task.CompletedTask;
    }
}
