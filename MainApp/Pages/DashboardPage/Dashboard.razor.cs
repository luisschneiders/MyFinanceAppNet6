using MainApp.Components.Spinner;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.DashboardPage;

public partial class Dashboard : ComponentBase
{
    [Inject]
    private SpinnerService _spinnerService { get; set; } = new();

    private bool _isLoadingChartBankAccountActive { get; set; } = false;
    private bool _isLoadingChartBankBalanceSum { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    public Dashboard()
    {
        
    }

    protected async override Task OnInitializedAsync()
    {

        _isLoadingChartBankAccountActive = true;
        await Task.Delay((int)Delay.DataLoading);

        _isLoadingChartBankBalanceSum = true;
        await Task.Delay((int)Delay.DataLoading);

        _isLoading = false;
        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _spinnerService.ShowSpinner();
        }

        await Task.CompletedTask;
    }
}
