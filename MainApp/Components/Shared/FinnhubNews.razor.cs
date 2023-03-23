using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Shared;

public partial class FinnhubNews : ComponentBase
{

    [Inject]
    private IFinnhubService _finnhubService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    Response<List<FinnhubNewsModel>> _newsList { get; set; } = default!;

    private bool _isLoading { get; set; } = true;

    public FinnhubNews()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _spinnerService.ShowSpinner();
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _newsList = await _finnhubService.GetNewsAsync();

            if (_newsList.Success is false)
            {
                _toastService.ShowToast($"{_newsList.ErrorMessage}. Feeds are unavailable", Theme.Danger);
            }

            _isLoading = false;
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
