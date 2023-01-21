using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Page;

public partial class PageWrapper : ComponentBase
{
    [Parameter]
    public RenderFragment? PageTopView { get; set; }

    [Parameter]
    public RenderFragment? PageMiddleView { get; set; }

    [Parameter]
    public RenderFragment? PageBottomView { get; set; }

    [Parameter]
    public string Styles { get; set; }

    public PageWrapper()
    {
        Styles = string.Empty;
    }
}
