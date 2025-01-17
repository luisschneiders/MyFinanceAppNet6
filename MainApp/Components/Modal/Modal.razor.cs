using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Modal;

public partial class Modal : ComponentBase
{
    // TODO: Add service for the modal

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public RenderFragment? Title { get; set; }

    [Parameter]
    public RenderFragment? Body { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public Size Size { get; set; } = Size.Md;

    [Parameter]
    public string ModalContentStyle { get; set; } = string.Empty;

    [Parameter]
    public string ModalBodyStyle { get; set; } = string.Empty;

    [Parameter]
    public bool IsCloseButtonVisible { get; set; } = true;

    [Parameter]
    public bool IsModalDialogCentered { get; set; } = true;

    [Parameter]
    public bool IsModalDialogBordered { get; set; } = true;

    [Parameter]
    public bool IsModalDialogScrollable { get; set; } = true;

    private ModalDisplay _modalDisplay { get; set; } = ModalDisplay.none;
    private string _modalStatus { get; set; } = string.Empty;
    private bool _showBackdrop { get; set; } = false;
    private Guid _modalId { get; set; } = Guid.Empty;

    public Modal()
    {
    }

    public async Task Open(Guid target)
    {
        _modalId = target;
        _modalDisplay = ModalDisplay.block;
        await Task.Delay((int)Delay.ModalOpen);
        _modalStatus = "show";
        _showBackdrop = true;

        StateHasChanged();
        await Task.CompletedTask;
    }

    public async Task Close(Guid target)
    {
        _modalId = target;
        _modalStatus = string.Empty;
        await Task.Delay((int)Delay.ModalClose);
        _modalDisplay = ModalDisplay.none;
        _showBackdrop = false;

        StateHasChanged();
        await Task.CompletedTask;
    }
}
