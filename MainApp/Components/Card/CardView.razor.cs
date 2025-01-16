using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Card;

public partial class CardView : ComponentBase
{
    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Link { get; set; }

    [Parameter]
    public string Styles { get; set; }

    public CardView()
    {
        Link = string.Empty;
        Styles = string.Empty;
    }
}
