using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Page;

public partial class PageMiddleWrapper : ComponentBase
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	[Parameter]
	public string Styles { get; set; }

	public PageMiddleWrapper()
	{
		Styles = string.Empty;
	}

}
