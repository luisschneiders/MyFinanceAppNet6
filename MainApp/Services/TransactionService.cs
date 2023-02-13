using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.NoSql;
using MyFinanceAppLibrary.DataAccess.Sql;

namespace MainApp.Services;

public class TransactionService : ITransactionService<TransactionModel>
{
    [Inject]
    private ITransactionData<TransactionModel> _transactionData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public TransactionService(ITransactionData<TransactionModel> transactionData, IUserData userData, AuthenticationStateProvider authProvider)
    {
        _transactionData = transactionData;
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
}
