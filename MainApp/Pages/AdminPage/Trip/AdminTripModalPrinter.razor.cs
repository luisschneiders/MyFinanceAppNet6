using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Trip;

public partial class AdminTripModalPrinter : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = new();

    [Inject]
    private IBrowserService _browserService { get; set; } = default!;

    [Inject]
    private IEnumHelper _enumHelper { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private PrintTripDTO _printTripDTO { get; set; } = new();
    private PrintPage _printPage { get; set; } = new();
    private bool _isLoading { get; set; } = true;

    private TripCategory[] _tripCategories { get; set; } = default!;

    public AdminTripModalPrinter()
    {
        _tripCategories = (TripCategory[])Enum.GetValues(typeof(TripCategory));
    }

    protected async override Task OnInitializedAsync()
    {
        await Task.CompletedTask;
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
            StateHasChanged();
        }
        await Task.CompletedTask;
    }

    public async Task OpenModalAsync(PrintTripDTO printTripDTO)
    {
        try
        {
            _modalTarget = Guid.NewGuid();
            _printTripDTO = printTripDTO;
            await Task.FromResult(_modal.Open(_modalTarget));
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task PrintAsync()
    {
        await Task.FromResult(_browserService.PrintWindow(_printPage.Trip));
        await Task.CompletedTask;
    }
    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private string UpdateTripCategoryTitle(ulong id)
    {
        string? title;
        if (id == (int)TripCategory.NotSpecified)
        {
            title = Label.NotSpecified;
        }
        else
        {
            title = _enumHelper.GetDescription(_tripCategories[id]);
        }
        return title;
    }
}
