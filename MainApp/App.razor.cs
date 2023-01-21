using System;
using Microsoft.AspNetCore.Components;

namespace MainApp;

public partial class App : ComponentBase
{
    [Inject]
    NavigationManager? _navManager { get; set; }

    private string _url { get; set; } = "/";

    private void BackToHome()
    {
        _navManager?.NavigateTo(_url);
    }
}
