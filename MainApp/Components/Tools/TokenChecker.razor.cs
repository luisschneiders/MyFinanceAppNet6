using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class TokenChecker : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IEssentialsAPIService _essentialsAPIService { get; set; } = default!;

    [Inject]
    private ISessionStorageService _sessionStorageService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private Response<string> _response = new();
    private bool _isTokenExpired { get; set; } = false;
    private bool _isTokenChecked { get; set; } = false;

    public TokenChecker()
    {
    }
    private async Task GetTokenAsync()
    {
        Response<string> response = await _essentialsAPIService.GetTokenWithBasicAuthAsync();

        if (response.Success is false)
        {
            _toastService.ShowToast(response.ErrorMessage, Theme.Danger);    
        }

        _response = response;

        await Task.CompletedTask;
    }

    private async Task CheckIsTokenExpiredAsync()
    {
        _isTokenChecked = true;
        _isTokenExpired = await _essentialsAPIService.IsTokenExpired();

        if (_isTokenExpired)
        {
            _response.Data = string.Empty;
            await _sessionStorageService.RemoveAsync(SessionStorage.EssentialsApiToken);
        }

        await Task.CompletedTask;
    }
}
