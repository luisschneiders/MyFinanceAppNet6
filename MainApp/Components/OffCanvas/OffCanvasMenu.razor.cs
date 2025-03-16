using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MainApp.Components.OffCanvas;

public partial class OffCanvasMenu : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Inject]
    private ToastService _toastService { get; set; } = new();

    private IJSObjectReference _objectReference { get; set; } = default!;

    public OffCanvasMenu()
	{
    }
}
