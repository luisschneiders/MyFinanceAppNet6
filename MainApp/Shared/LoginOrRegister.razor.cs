using Microsoft.AspNetCore.Components;

namespace MainApp.Shared;

public partial class LoginOrRegister : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    public LoginOrRegister()
	{
	}
}
