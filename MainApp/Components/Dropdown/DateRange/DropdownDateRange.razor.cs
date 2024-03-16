﻿using DateTimeLibrary.Enum;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Dropdown.DateRange;

public partial class DropdownDateRange : ComponentBase
{
    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback<DateTimeRange> OnSubmitSuccess { get; set; }

    [Parameter]
    public DateTimeRange DateTimeRange { get; set; }

    [Parameter]
    public bool IsDisplayLargeNone { get; set; } = false;

    [Parameter]
    public Theme IconColor { get; set; } = Theme.Dark;

    [Parameter]
    public Theme ButtonColor { get; set; } = Theme.Dark;

    [Parameter]
    public Size ButtonSize { get; set; } = Size.Md;

    [Parameter]
    public Position DropdownPosition { get; set; } = Position.Start;

    [Parameter]
    public string DropdownLabel { get; set; } = Label.NoDateAssigned;

    [Parameter]
    public PeriodRange PeriodRange { get; set; } = PeriodRange.Month;

    private bool _isValidDateRange { get; set; } = true;
    private bool _isDateChanged { get; set; } = false;

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
        DropdownLabel = await _dropdownDateRangeService.UpdateLabel(DateTimeRange);
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
            _isDateChanged = true;
            DropdownLabel = await _dropdownDateRangeService.UpdateLabel(DateTimeRange);
            await OnSubmitSuccess.InvokeAsync(DateTimeRange);
        }

        await Task.CompletedTask;
    }

    private async Task ResetDateTimeRange()
    {
        _isDateChanged = false;
        var dateTimeRange = new DateTimeRange();

        switch (PeriodRange)
        {
            case PeriodRange.Month:
                dateTimeRange = _dateTimeService.GetCurrentMonth();
                break;
            case PeriodRange.Year:
                dateTimeRange = _dateTimeService.GetCurrentYear();
                break;
            case PeriodRange.Week:
                dateTimeRange = _dateTimeService.GetCurrentWeek();
                break;
        }

        await OnSubmitSuccess.InvokeAsync(dateTimeRange);
        await Task.CompletedTask;
    }
}
