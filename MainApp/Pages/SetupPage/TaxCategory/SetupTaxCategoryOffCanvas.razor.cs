using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.SetupPage.TaxCategory;

public partial class SetupTaxCategoryOffCanvas : ComponentBase
{
    [Inject]
    private ITaxCategoryService<TaxCategoryModel> _taxCategoryService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Parameter]
    public IAppSettings AppSettings { get; set; } = default!;

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    [Parameter]
    public TaxCategoryModel DataModel { get; set; } = default!;

    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private InputFormAttributes _inputFormAttributes{ get; set; } = new();
    private TaxCategoryModel _taxCategoryModel { get; set; } = new();

    public SetupTaxCategoryOffCanvas()
    {
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _inputFormAttributes.Control = new()
                {
                    {
                        "class", $"form-control rounded{AppSettings.Form}"
                    }
                };

                _inputFormAttributes.PlainText = new()
                {
                    {
                        "class", $"form-control-plaintext"
                    }
                };

                _inputFormAttributes.Select = new()
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
        _taxCategoryModel = new();

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    public async Task EditRecordOffCanvasAsync(string id)
    {
        try
        {
            _taxCategoryModel = await _taxCategoryService.GetRecordById(id);
            if (_taxCategoryModel is not null)
            {
                await _offCanvasService.EditRecordAsync(id);
            }
            else
            {
                _taxCategoryModel = new();
                _toastService.ShowToast(Label.AppNoRecordFound, Theme.Danger);
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
            _taxCategoryModel = await _taxCategoryService.GetRecordById(id);
            if (_taxCategoryModel is not null)
            {
                await _offCanvasService.ViewRecordAsync(id);
            }
            else
            {
                _taxCategoryModel = new();
                _toastService.ShowToast(Label.AppNoRecordFound, Theme.Danger);
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
                await _taxCategoryService.CreateRecord(_taxCategoryModel);

                _taxCategoryModel.Id = await _taxCategoryService.GetLastInsertedId();
                _toastService.ShowToast(Label.AppMenuSetupTax+" "+Label.AppAdded, Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Edit)
            {
                await _taxCategoryService.UpdateRecord(_taxCategoryModel);
                _toastService.ShowToast(Label.AppMenuSetupTax+" "+Label.AppUpdated, Theme.Success);
            }
            else if (offCanvasViewType == OffCanvasViewType.Archive)
            {
                await _taxCategoryService.ArchiveRecord(_taxCategoryModel);
                _toastService.ShowToast(Label.AppMenuSetupTax+" "+Label.AppArchived, Theme.Success);
            }
            _isProcessing = false;

            DataModel = _taxCategoryModel;

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
        _taxCategoryModel = new();
        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }
}
