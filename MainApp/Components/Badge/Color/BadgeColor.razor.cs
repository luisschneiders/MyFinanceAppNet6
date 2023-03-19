using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Badge.Color;

public partial class BadgeColor : ComponentBase
{
    [Parameter]
    public string RGBColor { get; set; } = string.Empty;

    [Parameter]
    public string Description { get; set; } = string.Empty;

    public BadgeColor()
    {
    }
}
