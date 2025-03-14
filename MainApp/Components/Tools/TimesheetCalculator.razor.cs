using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class TimesheetCalculator : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter, EditorRequired]
    public InputFormAttributes InputFormAttributes{ get; set; } = new();

    private TimesheetModel _timesheetModel { get; set; } = new();

    public TimesheetCalculator()
    {
    }

    protected override async Task OnInitializedAsync()
    {
        _timesheetModel.TimeIn = DateTime.Now;
        _timesheetModel.TimeOut = DateTime.Now;

        await Task.CompletedTask;
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
