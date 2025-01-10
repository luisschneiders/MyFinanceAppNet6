using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Load;

public partial class Load : ComponentBase
{
    [Parameter]
    public string Message { get; set; } = Label.Loading;

    [Parameter]
    public Size Size { get; set; } = Size.Sm;

    [Parameter]
    public Theme Color { get; set; } = Theme.Primary;

    public Load()
    {
    }
}
