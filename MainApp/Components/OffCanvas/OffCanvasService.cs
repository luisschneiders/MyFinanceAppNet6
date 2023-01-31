using MyFinanceAppLibrary.Models;

namespace MainApp.Components.OffCanvas;

public class OffCanvasService : IOffCanvasService
{

    public OffCanvas OffCanvas { get; set; } = default!;
    private string _offCanvasTarget { get; set; } = string.Empty;

    private OffCanvasViewType _offCanvasViewType { get; set; }
    private Theme _offCanvasBadge { get; set; } = Theme.Info;

    public OffCanvasService()
    {
    }

    public async Task AddRecordAsync()
    {
        await SetOffCanvasStateAsync(OffCanvasViewType.Add, Theme.Success);
        await OffCanvas.Open(Guid.NewGuid().ToString());
        await Task.CompletedTask;
    }

    public async Task EditRecordAsync(string id)
    {
        try
        {
            _offCanvasTarget = id;

            await SetOffCanvasStateAsync(OffCanvasViewType.Edit, Theme.Danger);
            await OffCanvas.Open(_offCanvasTarget);

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
        }
        await Task.CompletedTask;
    }

    public async Task ViewRecordAsync(string id)
    {
        try
        {
            _offCanvasTarget = id;

            await SetOffCanvasStateAsync(OffCanvasViewType.View, Theme.Info);
            await OffCanvas.Open(_offCanvasTarget);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
        }
        await Task.CompletedTask;
    }

    public async Task ArchiveRecordAsync()
    {
        await SetOffCanvasStateAsync(OffCanvasViewType.Archive, Theme.Danger);
        await Task.CompletedTask;
    }

    public async Task UpdateFormStateAsync()
    {
        await SetOffCanvasStateAsync(OffCanvasViewType.Edit, Theme.Danger);
        await Task.CompletedTask;
    }

    public async Task CloseAsync()
    {
        await OffCanvas.Close(_offCanvasTarget);
        await Task.CompletedTask;
    }

    public OffCanvasViewType GetOffCanvasViewType()
    {
        return _offCanvasViewType;
    }

    public Theme GetOffCanvasBadge()
    {
        return _offCanvasBadge;
    }

    private async Task SetOffCanvasStateAsync(OffCanvasViewType offCanvasViewType, Theme theme)
    {
        _offCanvasViewType = offCanvasViewType;
        _offCanvasBadge = theme;
        await Task.CompletedTask;
    }
}
