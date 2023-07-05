using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage;

public partial class Setup : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    public Setup()
    {
    }
}
