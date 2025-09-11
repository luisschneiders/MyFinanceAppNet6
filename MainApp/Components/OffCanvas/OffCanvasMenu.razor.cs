using Microsoft.AspNetCore.Components;

namespace MainApp.Components.OffCanvas;

public partial class OffCanvasMenu : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    public OffCanvasMenu()
	{
    }
}
