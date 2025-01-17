using MainApp.Components.Spinner;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class SpinnerChecker : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Inject]
    private ISpinnerService _spinnerService { get; set; } = default!;


    public SpinnerChecker()
    {
    }

    private async Task ShowSpinnerAsync()
    {
        _spinnerService.ShowSpinner();
        await Task.CompletedTask;
    }

    private async Task HideSpinnerAsync()
    {
        _spinnerService.HideSpinner();
        await Task.CompletedTask;
    }
}
