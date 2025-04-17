using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Components.Tools;

public partial class InvestmentReturnCalculator : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter, EditorRequired]
    public InputFormAttributes InputFormAttributes{ get; set; } = new();

    private InvestmentReturnModel _investmentReturnModel { get; set; } = new();
    private InvestmentReturn[] _investmentReturnTypes { get; set; } = default!;

    public InvestmentReturnCalculator()
    {
        _investmentReturnTypes = (InvestmentReturn[])Enum.GetValues(typeof(InvestmentReturn));
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
