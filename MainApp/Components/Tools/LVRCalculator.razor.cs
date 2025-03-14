using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class LVRCalculator : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter, EditorRequired]
    public InputFormAttributes InputFormAttributes{ get; set; } = new();

    private LVRModel _lvrModel { get; set; } = new();

    public LVRCalculator()
    {
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await InvokeAsync(StateHasChanged);

            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }

        await Task.CompletedTask;
    }
}
