using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Constants;

namespace MainApp.Shared;

public partial class LoginDisplay : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    public LoginDisplay()
    {
    }
}
