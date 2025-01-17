using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Shared;

public partial class NavView : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    public NavView()
    {
    }
}
