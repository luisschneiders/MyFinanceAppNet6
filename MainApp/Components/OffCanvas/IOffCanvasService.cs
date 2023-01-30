namespace MainApp.Components.OffCanvas;

public interface IOffCanvasService
{
    OffCanvas OffCanvas { get; set; }

    Task AddRecordAsync();
    Task EditRecordAsync(string id);
    Task ViewRecordAsync(string id);
    Task ArchiveRecordAsync();
    Task UpdateFormStateAsync();
    Task CloseAsync();

    OffCanvasViewType GetOffCanvasViewType();
    Theme GetOffCanvasBadge();
}
