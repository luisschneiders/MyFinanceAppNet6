using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsAppearanceOffCanvas : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    //TODO: replace ILocalStorageService with IAppSettingsService
    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;

    private string _localStorageTheme { get; set; } = string.Empty;

    public SettingsAppearanceOffCanvas()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _localStorageTheme = await GetLocalStorageThemeAsync();
        }

        await Task.CompletedTask;
    }

    public async Task OpenAsync()
    {
        _offCanvasTarget = Guid.NewGuid().ToString();

        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));
        await Task.CompletedTask;
    }

    private async Task UpdateThemeAsync(ColorMode color)
    {
        try
        {
            await _localStorageService.SetAsync<string>(LocalStorage.AppTheme, color.ToString());
            await JS.InvokeVoidAsync("updateColorMode", color.ToString());

            _localStorageTheme = color.ToString();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task<string> GetLocalStorageThemeAsync()
    {
        string? localStorage = await _localStorageService.GetAsync<string>(LocalStorage.AppTheme);

        return await Task.FromResult(localStorage!);
    }
}
