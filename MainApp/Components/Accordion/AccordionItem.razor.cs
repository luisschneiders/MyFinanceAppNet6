using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Accordion;

public partial class AccordionItem : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string AccordionItemCSS { get; set; } = string.Empty;

    public AccordionItem()
    {
    }
}
