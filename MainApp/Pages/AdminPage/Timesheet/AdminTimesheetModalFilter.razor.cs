using MainApp.Components.Modal;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetModalFilter : ComponentBase
{
    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IDropdownMultiSelectService _dropDownMultiSelectService { get;set; } = default!;

    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;
    [Inject]
    private ITimesheetService<TimesheetModel> _timesheetService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected IAppSettings _appSettings { get; set; } = default!;

    [Parameter]
    public EventCallback<MultiFilterTimesheetDTO> OnSubmitFilterSuccess { get; set; }
    
    private MultiFilterTimesheetDTO _multiFilterTimesheetDTO { get; set; } = new();
    private List<CheckboxItemModel> _companies { get; set; } = new();
    private List<CheckboxItemModel> _statuses { get; set; } = new();
    private Modal _modal { get; set; } = new();
    private Guid _modalTarget { get; set; }

    public AdminTimesheetModalFilter()
    {
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

    private async Task FetchDataAsync()
    {
        try
        {
            _companies = await _companyService.GetRecordsForFilter();
            _statuses = await _timesheetService.GetRecordsForFilter();
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

    private async Task ResetAllFilters()
    {
        await UncheckAll();
        await RemoveAllFilters();

        await Task.CompletedTask;
    }

    private async Task RemoveDropdownFilterCompany()
    {
        _multiFilterTimesheetDTO.CompanyId = new();
        _companies = await _dropDownMultiSelectService.UncheckAll(_companies);

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTimesheetDTO);

        await Task.CompletedTask;
    }
    private async Task RemoveDropdownFilterStatus()
    {
        _multiFilterTimesheetDTO.StatusId = new();
        _statuses = await _dropDownMultiSelectService.UncheckAll(_statuses);

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTimesheetDTO);

        await Task.CompletedTask;
    }

    private async Task UncheckAll()
    {
        _companies = await _dropDownMultiSelectService.UncheckAll(_companies);
        _statuses = await _dropDownMultiSelectService.UncheckAll(_statuses);

        await Task.CompletedTask;
    }

    private async Task RemoveAllFilters()
    {
        _multiFilterTimesheetDTO.CompanyId = new();
        _multiFilterTimesheetDTO.StatusId = new();

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTimesheetDTO);

        await Task.CompletedTask;
    }

    private async void OnCheckboxChangedCompany(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel company = _companies.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterTimesheetDTO.CompanyId.Add(id);
            company.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterTimesheetDTO.CompanyId.Remove(id);
            company.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTimesheetDTO);
        await Task.CompletedTask;
    }
    private async void OnCheckboxChangedStatus(ChangeEventArgs e, ulong id)
    {
        CheckboxItemModel status = _statuses.FirstOrDefault(i => i.Id == id)!;

        if (e.Value is true)
        {
            _multiFilterTimesheetDTO.StatusId.Add(id);
            status.IsChecked = true;
        }
        else if (e.Value is false)
        {
            _multiFilterTimesheetDTO.StatusId.Remove(id);
            status.IsChecked = false;
        }

        await OnSubmitFilterSuccess.InvokeAsync(_multiFilterTimesheetDTO);
        await Task.CompletedTask;
    }
}
