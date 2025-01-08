using Microsoft.AspNetCore.Components;
using MainApp.Shared;
using MainApp.Components.Spinner;

namespace MainApp.Components.Spinner;

public partial class Spinner : ComponentBase, IDisposable
{
    [Inject]
    private SpinnerService _spinnerService { get; set; }

    [Parameter]
    public Size Size { get; set; }

    [Parameter]
    public Theme Color { get; set; }

    private string _componentSize { get; set; }
    private string _componentColor { get; set; }

    private bool _isVisible { get; set; }

    public Spinner()
    {
        _spinnerService = new();
        _componentSize = string.Empty;
        _componentColor = string.Empty;
    }

    protected override void OnInitialized()
    {
        _componentSize = Size.ToString().ToLower();
        _componentColor = Color.ToString().ToLower();
        _spinnerService.OnShow += ShowSpinner;
        _spinnerService.OnHide += HideSpinner;
    }

    public void ShowSpinner()
    {
        _isVisible = true;
        InvokeAsync(() => StateHasChanged());
    }

    public void HideSpinner()
    {
        _isVisible = false;
        InvokeAsync(() => StateHasChanged());
    }

    public void Dispose()
    {
        _spinnerService.OnShow -= ShowSpinner;
        _spinnerService.OnHide -= HideSpinner;
        GC.SuppressFinalize(this);
    }
}
