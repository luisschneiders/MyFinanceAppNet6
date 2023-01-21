using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Toast;

public partial class Toast : ComponentBase, IDisposable
{
    [Inject]
    private ToastService _toastService { get; set; }

    private Guid _toastId { get; set; }
    private string _message { get; set; }
    private bool _isVisible { get; set; }
    private string _backgroundColor { get; set; }

    public Toast()
    {
        _toastService = new();
        _toastId = Guid.NewGuid();
        _message = string.Empty;
        _backgroundColor = string.Empty;
    }

    protected override void OnInitialized()
    {
        _toastService.OnShow += ShowToast;
        _toastService.OnHide += HideToast;
    }

    private void ShowToast(string message, Theme theme)
    {

        _backgroundColor = theme.ToString().ToLower();
        _message = message;

        _isVisible = true;
        InvokeAsync(() => StateHasChanged());
    }

    private void HideToast()
    {
        _isVisible = false;
        InvokeAsync(() => StateHasChanged());
    }

    public void Dispose()
    {
        _toastService.OnShow -= ShowToast;
    }
}
