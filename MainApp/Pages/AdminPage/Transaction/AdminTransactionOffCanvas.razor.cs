using System;
using System.ComponentModel.DataAnnotations;
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

    [Parameter]
    public EventCallback OnSubmitSuccess { get; set; }

    private TransactionModel _transactionModel { get; set; } = new();
    private List<BankModel> _activeBanks { get; set; } = new();
    private List<TransactionCategoryModel> _activeTransactionCategories { get; set; } = new();

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

    public async Task AddRecordOffCanvasAsync()
    {
        _transactionModel = new();
        _transactionModel.TDate = DateTime.Now;

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

            switch (_transactionModel.TCategoryTypeModel.ActionType)
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

    private async Task UpdateFormStateAsync()
    {
        await _offCanvasService.UpdateFormStateAsync();
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
                _transactionModel.TCategoryTypeModel = result;
            }
            else
            {
                _transactionModel.TCategoryTypeModel = new();
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
