using MainApp.Components.OffCanvas;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyFinanceAppLibrary.Models;

namespace MainApp.Pages.AdminPage.Transaction;

public partial class AdminTransactionOffCanvas : ComponentBase
{
    [Inject]
    private ITransactionService<TransactionModel> _transactionService { get; set; } = default!;

    [Inject]
    private IBankService<BankModel> _bankService { get; set; } = default!;

    [Inject]
    private ITransactionCategoryService<TransactionCategoryModel> _transactionCategoryService { get; set; } = default!;

    [Inject]
    private IOffCanvasService _offCanvasService { get; set; } = default!;

    [Inject]
    private ToastService _toastService { get; set; } = default!;

    [Inject]
    private IAnimationService _animationService { get; set; } = default!;


    [CascadingParameter(Name = "AppSettings")]
    protected AppSettings _appSettings { get; set; } = new();

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private TransactionModel _transactionModel { get; set; } = new();
    private List<BankModel> _activeBanks { get; set; } = new();
    private List<TransactionCategoryModel> _activeTransactionCategories { get; set; } = new();

    private Dictionary<string, object> _inputFormControlAttributes = default!;
    private Dictionary<string, object> _inputFormControlDisabledAttributes = default!;
    private Dictionary<string, object> _inputFormSelectAttributes = default!;

    private bool _shouldRender { get; set; } = true;
    private bool _displayErrorMessages { get; set; } = false;
    private bool _isProcessing { get; set; } = false;
    private bool _isLoading { get; set; } = true;

    private string _actionType { get; set; } = string.Empty;

    public AdminTransactionOffCanvas()
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
                _inputFormControlDisabledAttributes = new()
                {
                    {
                        "class", $"form-control rounded{_appSettings.Form} bg-secondary-subtle"
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

    protected override bool ShouldRender()
    {
        if (_shouldRender)
        {
            Task.FromResult(FetchDataAsync());
        }

        return _shouldRender;
    }

    public async Task AddRecordOffCanvasAsync(DateTime date)
    {
        _transactionModel = new()
        {
            TDate = date
        };

        await _offCanvasService.AddRecordAsync();
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        try
        {
            _activeBanks = await _bankService.GetRecordsActive();
            _activeTransactionCategories = await _transactionCategoryService.GetRecordsActive();
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

    private async Task HandleValidSubmitAsync()
    {
        try
        {
            _displayErrorMessages = false;
            _isProcessing = true;

            switch (_transactionModel.TransactionCategoryModel.ActionType)
            {
                case "T":
                    await _transactionService.CreateRecordTransfer(_transactionModel);
                    break;
                case "D":
                    await _transactionService.CreateRecordDebit(_transactionModel);
                    break;
                case "C":
                    await _transactionService.CreateRecordCredit(_transactionModel);
                    break;
            }

            _isProcessing = false;
            _toastService.ShowToast("Transaction added!", Theme.Success);

            if (_transactionModel.TransactionCategoryModel.ActionType == TransactionActionType.C.ToString())
            {
                await _animationService.ConfettiTransaction();
            }

            await OnSubmitSuccess.InvokeAsync();
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

    private async Task CloseOffCanvasAsync()
    {
        _transactionModel = new();
        _activeBanks = new();
        _actionType = string.Empty;

        await _offCanvasService.CloseAsync();
        await Task.CompletedTask;
    }

    private async void OnValueChangedCategoryType(ChangeEventArgs args)
    {
        var valueChanged = args?.Value;

        if (valueChanged is not null)
        {
            ulong categotyId = ulong.Parse(valueChanged.ToString()!);

            if (categotyId != 0)
            {
                var result = _activeTransactionCategories.Find(c => c.Id == categotyId);
                _actionType = result!.ActionType;
                _transactionModel.TransactionCategoryModel = result;

                if (_actionType == TransactionActionType.T.ToString())
                {
                    _transactionModel.Comments = $"Comments are disabled for Transfers";
                }
                else
                {
                    _transactionModel.Comments = string.Empty;
                }
            }
            else
            {
                _transactionModel.TransactionCategoryModel = new();
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    private async void OnValueChangedFromBank(ChangeEventArgs args)
    {
        var valueChanged = args?.Value;

        if (valueChanged is not null)
        {
            ulong bankId = ulong.Parse(valueChanged.ToString()!);

            if (bankId != 0)
            {
                _transactionModel.FromBankModel = _activeBanks.Find(c => c.Id == bankId);
            }
            else
            {
                _transactionModel.FromBankModel = new();
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }

    private async void OnValueChangedToBank(ChangeEventArgs args)
    {
        var valueChanged = args?.Value;

        if (valueChanged is not null)
        {
            ulong bankId = ulong.Parse(valueChanged.ToString()!);

            if (bankId != 0)
            {
                _transactionModel.ToBankModel = _activeBanks.Find(c => c.Id == bankId);
            }
            else
            {
                _transactionModel.ToBankModel = new();
            }

            StateHasChanged();
        }

        await Task.CompletedTask;
    }
}
