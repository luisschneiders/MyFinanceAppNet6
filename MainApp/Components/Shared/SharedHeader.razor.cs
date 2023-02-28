using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Shared;

public partial class SharedHeader : ComponentBase
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public Theme TitleColor { get; set; } = Theme.Secondary;

    [Parameter]
    public string Icon { get; set; } = string.Empty;

    [Parameter]
    public Theme IconColor { get; set; } = Theme.Primary;

    [Parameter]
    public Theme BadgeBackgroundColor { get; set; } = Theme.Primary;

    [Parameter]
    public string BadgeTitle { get; set; } = string.Empty;

    public SharedHeader()
    {
    }
}
