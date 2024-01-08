using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.Company;

public partial class SetupCompanyOffCanvas : ComponentBase
{
    [Inject]
    private ICompanyService<CompanyModel> _companyService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public CompanyModel DataModel { get; set; } = default!;

    [Parameter]
    public AppSettings AppSettings { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;

    private Dictionary<string, object> _inputFormControlAttributes = default!;
    private Dictionary<string, object> _inputFormControlPlainTextAttributes = default!;
    private Dictionary<string, object> _inputFormSelectAttributes = default!;

    private CompanyModel _companyModel { get; set; } = new();

    private CompanyType[] _companyTypes { get; set; } = default!;

    public SetupCompanyOffCanvas()
    {
        _companyTypes = (CompanyType[])Enum.GetValues(typeof(CompanyType));
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _inputFormControlAttributes = new()
                {
                    {
                        "class", $"form-control rounded{AppSettings.Form}"
                    }
                };
                _inputFormControlPlainTextAttributes = new()
                {
                    {
                        "class", $"form-control-plaintext"
                    }
                };
                _inputFormSelectAttributes = new()
                {
                    {
                        "class", $"form-select rounded{AppSettings.Form}"
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
        _companyModel = new();

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _companyModel = await _companyService.GetRecordById(id);
            if (_companyModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _companyModel = new();
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
            _companyModel = await _companyService.GetRecordById(id);

            if (_companyModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _companyModel = new();
                _toastService.ShowToast("No record found!", Theme.Danger);
            }
        }
        catch (Exception ex)
        {
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

            if (offCanvasViewType == OffCanvasViewType.Add)
            {
                await _companyService.CreateRecord(_companyModel);

                _companyModel.Id = await _companyService.GetLastInsertedId();
                _toastService.ShowToast("Company added!", Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Edit)
            {
                await _companyService.UpdateRecord(_companyModel);
                _toastService.ShowToast("Company updated!", Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Archive)
            {
                await _companyService.ArchiveRecord(_companyModel);
                _toastService.ShowToast("Company archived!", Theme.Success);
            }
            _isProcessing = false;

            DataModel = _companyModel;

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
        _companyModel = new();
        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
