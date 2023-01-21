using System;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Card;

public partial class CardTop : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

}
