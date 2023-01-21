using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Badge;

public partial class Badge : ComponentBase, IBadge
{

    [Parameter]
    public Theme BackgroundColor { get; set; } = Theme.Info;

    [Parameter]
    public string Message { get; set; } = string.Empty;

    public Badge()
    {
    }
}
