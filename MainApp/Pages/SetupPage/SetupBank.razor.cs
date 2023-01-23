using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;

namespace MainApp.Pages.SetupPage;

public partial class SetupBank : ComponentBase
{
    [Inject]
    IBankService _bankService { get; set; } = default!;

    private List<BankModel> _banks { get; set; } = new();

    public SetupBank()
    {
    }

    protected async override Task OnInitializedAsync()
    {
        await FetchDataAsync();
        await Task.CompletedTask;
    }

    private async Task SearchAsync()
    {
        await Task.CompletedTask;
    }

    private async Task AddRecordAsync()
    {
        await Task.CompletedTask;
    }

    private async Task FetchDataAsync()
    {
        _banks = await _bankService.GetAllBanksByUserId();

        //if (_paginationResponse.Success == false)
        //{
        //    _toastService.ShowToast(_paginationResponse.ErrorMessage, Theme.Danger);
        //}

        //if (_covid19Service.HasDataAsync())
        //{
        //    _covid19Countries = await _covid19Service.GetDataListAsync();
        //    _paginationService.Filter = paginationFilter;
        //}

        await Task.CompletedTask;
    }

    private async Task UpdateStatusAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }

    private async Task ViewRecordAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }

    private async Task EditRecordAsync(BankModel bankModel)
    {
        await Task.CompletedTask;
    }

}
