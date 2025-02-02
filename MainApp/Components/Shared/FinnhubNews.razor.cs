using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Shared;

public partial class FinnhubNews : ComponentBase
{

    [Inject]
    private IFinnhubService _finnhubService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

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
                _toastService.ShowToast($"{_newsList.ErrorMessage}", Theme.Danger);
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

    private async Task RefreshNewsAsync()
    {
        _isLoading = true;
        _newsList = new();

        await InvokeAsync(StateHasChanged);
        await FetchDataAsync();
        await Task.CompletedTask;
    }
}
