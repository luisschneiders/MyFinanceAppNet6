using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage;

public partial class Setup : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    public Setup()
    {
    }
}
