using Microsoft.AspNetCore.Components;

namespace MainApp.Shared;

public partial class LoginOrRegister : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    public LoginOrRegister()
	{
	}
}
