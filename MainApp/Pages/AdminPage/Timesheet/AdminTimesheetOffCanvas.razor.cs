﻿using DateTimeLibrary.Models;
using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.AdminPage.Timesheet;

public partial class AdminTimesheetOffCanvas : ComponentBase
{
    [Inject]
    private ITimesheetService<TimesheetModel> _timesheetService { get; set; } = default!;

    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isDifferentRate { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    private TimesheetModel _timesheetModel { get; set; } = new();
    private List<CompanyModel> _activeCompanies { get; set; } = new();

    private Dictionary<string, object> _inputFormControlAttributes = default!;
    private Dictionary<string, object> _inputFormSelectAttributes = default!;
    private Dictionary<string, object> _inputFormControlAttributesPlainText = default!;

    public AdminTimesheetOffCanvas()
    {
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await FetchDataAsync();

                _inputFormControlAttributes = new()
                {
                    {
                        "class", $"form-control rounded{_appSettings.Form}"
                    }
                };
                _inputFormSelectAttributes = new()
                {
                    {
                        "class", $"form-select rounded{_appSettings.Form}"
                    }
                };
                _inputFormControlAttributesPlainText = new()
                {
                    {
                        "class", $"form-control-plaintext"
                    }
                };
            }
            catch (Exception ex)
            {
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
        }
        await Task.CompletedTask;
    }

    public async Task AddRecordOffCanvasAsync()
    {
        _timesheetModel = new()
        {
            TimeIn = DateTime.Now,
            TimeOut = DateTime.Now
        };

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _timesheetModel = await _timesheetService.GetRecordById(id);
            if (_timesheetModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _timesheetModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    public async Task ViewRecordOffCanvasAsync(string id)
    {
        try
        {
            _timesheetModel = await _timesheetService.GetRecordById(id);
            if (_timesheetModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _timesheetModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
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
            _activeCompanies = await _companyService.GetRecordsActive();
            _isLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _isLoading = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task UpdateFormStateAsync()
    {
        await _offCanvasService.UpdateFormStateAsync();
        await Task.CompletedTask;
    }

    private async Task ArchiveAsync()
    {
        try
        {
            await _offCanvasService.ArchiveRecordAsync();
        }
        catch (Exception ex)
        {
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }

        await Task.CompletedTask;
    }

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            var offCanvasViewType = _offCanvasService.GetOffCanvasViewType();

            switch (offCanvasViewType)
            {
                case OffCanvasViewType.Add:
                    await _timesheetService.CreateRecord(_timesheetModel);
                    _toastService.ShowToast("Timesheet added!", Theme.Success);
                    break;
                case OffCanvasViewType.Edit:
                    await _timesheetService.UpdateRecord(_timesheetModel);
                    _toastService.ShowToast("Timesheet updated!", Theme.Success);
                    break;
                case OffCanvasViewType.Archive:
                    await _timesheetService.ArchiveRecord(_timesheetModel);
                    _toastService.ShowToast("Timesheet archived!", Theme.Success);
                    break;
            }

            _isProcessing = false;

            await OnSubmitSuccess.InvokeAsync();
            await Task.Delay((int)Delay.DataSuccess);
            await CloseOffCanvasAsync();
        }
        catch (Exception ex)
        {
            _isProcessing = false;
            _toastService.ShowToast(ex.Message, Theme.Danger);
        }
        await Task.CompletedTask;
    }

    private async Task HandleInvalidSubmitAsync()
    {
        _isProcessing = false;
        _displayErrorMessages = true;
        await Task.CompletedTask;
    }

    private async Task CloseOffCanvasAsync()
    {
        _timesheetModel = new();

        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }

    private async void OnValueChanged(ChangeEventArgs args)
    {
        var valueChanged = args?.Value;

        if (valueChanged is not null)
        {
            ulong companyId = ulong.Parse(valueChanged.ToString()!);

            if (companyId > 0)
            {
                _timesheetModel.HourRate = await _companyService.GetHourRate(companyId.ToString());
                StateHasChanged();
            }
        }
    }
}
