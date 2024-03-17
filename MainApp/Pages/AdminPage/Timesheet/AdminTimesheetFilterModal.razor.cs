using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetFilterModal : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownFilterService _dropdownFilterService { get; set; } = default!;

    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback<FilterTimesheetDTO> OnSubmitFilterSuccess { get; set; }

    private List<CompanyModel> _companies { get; set; } = new();
    private FilterModel _filterCompanyModel { get; set; } = new();
    private FilterTimesheetDTO _filterTimesheetDTO { get; set; } = new();
    private CompanyModel _filterCompany { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }
    private string _dropdownFilterLabelCompany { get; set; } = Label.NoFilterAssigned;

    public AdminTimesheetFilterModal()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        _dropdownFilterLabelCompany = await _dropdownFilterService.UpdateLabel(Label.FilterByCompany);

        await Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await FetchDataAsync();
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            await InvokeAsync(StateHasChanged);
        }

        await Task.CompletedTask;
    }

    public async Task OpenModalAsync(bool isFilterApplied)
    {
        try
        {
            _modalTarget = Guid.NewGuid();

            if (isFilterApplied is false)
            {
                await ResetAllFilters();
            }

            await Task.FromResult(_modal.Open(_modalTarget));
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task CloseModalAsync()
    {
        await Task.FromResult(_modal.Close(_modalTarget));
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _companies = await _companyService.GetRecords();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task ResetAllFilters()
    {
        await RemoveDropdownFilterCompany();
        await Task.CompletedTask;
    }

    private async Task ResetDropdownFilterCompany()
    {
        await RemoveDropdownFilterCompany();
        
        _toastService.ShowToast("Filter for company removed!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTimesheetDTO);
        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterCompany()
    {
        _filterCompany = new();
        _filterTimesheetDTO.CompanyId = 0;
        _filterCompanyModel = await _dropdownFilterService.ResetModel();
        _dropdownFilterLabelCompany = await _dropdownFilterService.UpdateLabel(Label.FilterByCompany);

        await Task.CompletedTask;
    }

    private async Task RefreshDropdownFilterCompany(ulong id)
    {
        _filterTimesheetDTO.CompanyId = id;
        _filterCompany = _companies.First(i => i.Id == id);
        _filterCompanyModel = await _dropdownFilterService.SetModel(_filterCompany.Id, _filterCompany.Description);
        _dropdownFilterLabelCompany = await _dropdownFilterService.UpdateLabel(_filterCompany.Description);
        _toastService.ShowToast("Filter updated!", Theme.Info);

        await OnSubmitFilterSuccess.InvokeAsync(_filterTimesheetDTO);
        await Task.CompletedTask;
    }
}
