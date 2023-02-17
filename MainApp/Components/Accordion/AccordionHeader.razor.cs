using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Accordion;

public partial class AccordionHeader : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Id { get; set; } = string.Empty;

    public AccordionHeader()
    {
    }
}
