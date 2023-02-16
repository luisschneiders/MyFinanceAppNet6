using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;
using MyFinanceAppLibrary.DataAccess.Sql;
using MyFinanceAppLibrary.Models;

namespace MainApp.Services;

public class TransactionService : ITransactionService<TransactionModel>
{
    [Inject]
    private ITransactionData<TransactionModel> _transactionData { get; set; } = default!;

    [Inject]
    private IBankData<BankModel> _bankData { get; set; } = default!;

    [Inject]
    private ITransactionCategoryData<TransactionCategoryModel> _transactionCategoryData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public TransactionService(
        ITransactionData<TransactionModel> transactionData,
        IBankData<BankModel> bankData,
        ITransactionCategoryData<TransactionCategoryModel> transactionCategoryData,
        IUserData userData,
        AuthenticationStateProvider authProvider)
    {
        _transactionData = transactionData;
        _bankData = bankData;
        _transactionCategoryData = transactionCategoryData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public Task ArchiveRecord(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecord(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public async Task CreateRecordCredit(TransactionModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.Action = TransactionActionType.C.ToString();
            model.Label = model.TCategoryTypeModel.ActionType;
            model.UpdatedBy = user.Id;

            await _transactionData.CreateRecordCredit(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecordDebit(TransactionModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.Action = TransactionActionType.D.ToString();
            model.Label = model.TCategoryTypeModel.ActionType;
            model.UpdatedBy = user.Id;

            await _transactionData.CreateRecordDebit(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecordTransfer(TransactionModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.Label = model.TCategoryTypeModel.ActionType;
            model.Comments = $"Transfer from {model.FromBankModel.Description} to {model.ToBankModel.Description}";
            model.UpdatedBy = user.Id;

            await _transactionData.CreateRecordTransfer(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<TransactionModel> GetRecordById(string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TransactionModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<TransactionModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public Task<List<TransactionModel>> GetSearchResults(string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}
