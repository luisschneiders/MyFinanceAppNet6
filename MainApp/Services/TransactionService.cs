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

            TransactionModel recordModel = new()
            {
                TDate = model.TDate,
                FromBank = model.FromBank,
                TCategoryType = model.TCategoryType,
                //Action = "C", // Set it here or in the db?
                //Label = transactionCategoryModel.ActionType,
                Amount = model.Amount,
                Comments = model.Comments,
                UpdatedBy = user.Id
            };

            await _transactionData.CreateRecord(recordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task CreateRecordDebit(TransactionModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecordTransfer(TransactionModel model)
    {
        throw new NotImplementedException();
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
