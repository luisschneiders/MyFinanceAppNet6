using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Card;

public partial class CardBottom : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
