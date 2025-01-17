using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Shared;

public partial class LoginDisplay : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    public LoginDisplay()
    {
    }
}
