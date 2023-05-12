using DateTimeLibrary.Models;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Shared;

public partial class MapLocationExpense : ComponentBase
{
    [Parameter]
    public MapMarkerColor Color { get; set; } = MapMarkerColor.Orange;

    [Parameter]
    public MapSize Width { get; set; } = MapSize.Width400;

    [Parameter]
    public MapSize Height { get; set; } = MapSize.Height250;

    [Parameter]
    public PeriodRange PeriodRange { get; set; } = PeriodRange.Month;

    [Parameter]
    public DateTimeRange DateRange { get; set; } = new();

    [Inject]
    private IExpenseService<ExpenseModel> _expenseService { get; set; } = default!;

    [Inject]
    private IGoogleService _googleService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private IDropdownDateRangeService _dropdownDateRangeService { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();
    private string _dropdownLabel { get; set; } = Label.NoDateAssigned;

    private string _imageURL { get; set; } = string.Empty;
    private bool _isLoading { get; set; } = true;

    public MapLocationExpense()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dateTimeRange = DateRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(_dateTimeRange);
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            GoogleMapStaticModel model = await _expenseService.GetLocationExpense(_dateTimeRange, Color, Width, Height);
            Response<string> staticImage = await _googleService.GetMapStaticImage(model);

            _imageURL = staticImage.Data;
            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task DropdownDateRangeRefresh(DateTimeRange dateTimeRange)
    {
        _dateTimeRange = dateTimeRange;
        _dropdownLabel = await _dropdownDateRangeService.UpdateLabel(dateTimeRange);
        _toastService.ShowToast("Date range has changed!", Theme.Info);

        await FetchDataAsync();

        await Task.CompletedTask;
    }
}
