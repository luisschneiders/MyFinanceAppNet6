using Microsoft.AspNetCore.Components;

namespace MainApp.Components.OffCanvas;

public interface IOffCanvas
{
    RenderFragment? ChildContent { get; set; }
    Position PositionX { get; set; }
    OffCanvasSize Size { get; set; }
    string Title { get; set; }

    Task Close(string target);
    Task Open(string target);
}
