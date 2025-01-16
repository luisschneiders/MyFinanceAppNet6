using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionChart : ComponentBase
{
    [Inject]
    private IDateTimeService _dateTimeService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IBrowserService _browserService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    private DateTimeRange _dateTimeRange { get; set; } = new();

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }

    public AdminTransactionChart()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _modalTarget = Guid.NewGuid();
        _dateTimeRange = _dateTimeService.GetCurrentMonth();
        await Task.CompletedTask;
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await Task.FromResult(_modal.Open(_modalTarget));
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            StateHasChanged();
        }
        await Task.CompletedTask;
    }

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_browserService.CloseTab());
        await Task.CompletedTask;
    }
}
