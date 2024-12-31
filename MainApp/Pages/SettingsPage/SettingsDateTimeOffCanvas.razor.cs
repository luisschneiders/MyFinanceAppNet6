using System.Globalization;
using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SettingsPage;

public partial class SettingsDateTimeOffCanvas : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IAppSettingsService _appSettingsService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private OffCanvas _offCanvas { get; set; } = new();
    private string _offCanvasTarget { get; set; } = string.Empty;
    private string[] _dayNames { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
    private string _localStorageStartOfWeek { get; set; } = string.Empty;
    private bool _startOfWeekIsInvalid { get; set; } = false;

    public SettingsDateTimeOffCanvas()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _localStorageStartOfWeek = await GetLocalStorageStartOfWeekAsync();
        }

        await Task.CompletedTask;
    }

    public async Task OpenAsync()
    {
        _offCanvasTarget = Guid.NewGuid().ToString();

        await ResetDefaults();

        await Task.FromResult(_offCanvas.Open(_offCanvasTarget));

        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        await Task.FromResult(_offCanvas.Close(_offCanvasTarget));

        await Task.CompletedTask;
    }

    private async Task<string> GetLocalStorageStartOfWeekAsync()
    {
        string startOfWeek = await _appSettingsService.GetLocalStorageStartOfWeek();

        return await Task.FromResult(startOfWeek!);
    }

    private async Task ResetDefaults()
    {
        _startOfWeekIsInvalid = false;

        await Task.CompletedTask;
    }

    private async Task UpdateWeekDayAsync(string weekDay)
    {
        try
        {
            _startOfWeekIsInvalid = false;

            if (string.IsNullOrWhiteSpace(weekDay))
            {
                _startOfWeekIsInvalid = true;
                return;
            }

            await _appSettingsService.SetLocalStorageStartOfWeek(weekDay);

            _localStorageStartOfWeek = weekDay;
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }
}
