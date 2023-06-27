using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.Date;

public partial class DropdownDateMonthYear : ComponentBase
{
    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [Inject]
    private IDropdownDateMonthYearService _dropdownDateMonthYearService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Parameter]
    public EventCallback<DateTimeRange> OnSubmitSuccess { get; set; }

    [Parameter]
    public DateTimeRange DateTimeRange { get; set; }

    [Parameter]
    public Theme IconColor { get; set; } = Theme.Success;

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Light;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public string DropdownLabel { get; set; } = Label.NoDateAssigned;

    private string _buttonRadius { get; set; } = Radius.Default;

    public DropdownDateMonthYear()
    {
        var now = DateTime.Now;
        var startDate = new DateTime(now.Year, now.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        DateTimeRange = new DateTimeRange
        {
            Start = DateTime.Now,
            End = DateTime.Now
        };
    }

    protected override async Task OnInitializedAsync()
    {
        DropdownLabel = await _dropdownDateMonthYearService.UpdateLabel(DateTimeRange);
        await Task.CompletedTask;
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

    private async Task ChangeDateAsync()
    {
        DateTimeRange dateTimeRange = DateTimeRange;
        dateTimeRange.End = _dateTimeService.GetLastDayOfMonth(DateTimeRange);

        DropdownLabel = await _dropdownDateMonthYearService.UpdateLabel(DateTimeRange);
        await OnSubmitSuccess.InvokeAsync(dateTimeRange);
        await Task.CompletedTask;
    }
}
