using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Card;

public partial class CardView : ComponentBase
{
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
