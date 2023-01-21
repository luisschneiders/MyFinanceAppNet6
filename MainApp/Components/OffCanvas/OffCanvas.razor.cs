using Microsoft.AspNetCore.Components;

namespace MainApp.Components.OffCanvas;

	public partial class OffCanvas : ComponentBase
	{

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    // start or end
    [Parameter]
    public Position PositionX { get; set; } = Position.End;

    [Parameter]
    public OffCanvasSize Size { get; set; }

    [Parameter]
    public string Title { get; set; } = Label.AppTitle;

    private string _offCanvasId { get; set; } = string.Empty;
    private string _offCanvasClass { get; set; } = string.Empty;
    private bool _showBackdrop { get; set; } = false;

    public OffCanvas()
	{
    }

    public async Task Open(string target)
    {

        _offCanvasId = target;
        await Task.Delay((int)Delay.OffCanvasOpen);
        _offCanvasClass = "show";
        _showBackdrop = true;

        StateHasChanged();
    }

    public async Task Close(string target)
    {
        _offCanvasId = target;
        _offCanvasClass = string.Empty;
        await Task.Delay((int)Delay.OffCanvasClose);
        _showBackdrop = false;

        StateHasChanged();
    }
}
