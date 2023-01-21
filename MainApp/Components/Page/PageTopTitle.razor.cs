using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Page;

public partial class PageTopTitle : ComponentBase
{

    [Parameter]
    public string Title { get; set; }

    public PageTopTitle()
	{
        Title = string.Empty;
    }
}
