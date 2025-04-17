using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Loader;

public partial class Loader : ComponentBase
{
    [Parameter]
    public string Message { get; set; } = Label.AppLoading;

    [Parameter]
    public Size Size { get; set; } = Size.Sm;

    [Parameter]
    public Theme Color { get; set; } = Theme.Primary;

    public Loader()
    {
    }
}
