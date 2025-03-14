
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.ToolsPage;

public partial class Tools : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private InputFormAttributes _inputFormAttributes{ get; set; } = new();

    public Tools()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _inputFormAttributes.Control = new()
        {
            {
                "class", $"form-control rounded{_appSettings.Form}"
            }
        };

        await Task.CompletedTask;
    }
}
