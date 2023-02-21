using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Accordion;

public partial class AccordionItem : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public AccordionItem()
    {
    }
}
