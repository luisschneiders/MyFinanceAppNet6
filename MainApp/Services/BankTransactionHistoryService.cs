
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class BankTransactionHistoryService : IBankTransactionHistoryService<BankTransactionHistoryModel>
{
    [Inject]
    private IBankTransactionHistoryData<BankTransactionHistoryModel> _bankTransactionHistoryData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;


    private List<BankTransactionHistoryListDTO> _recordsByDateRange { get; set; } = new();


    public BankTransactionHistoryService(
        IBankTransactionHistoryData<BankTransactionHistoryModel> bankTransactionHistoryData,
        IUserData userData,
        AuthenticationStateProvider authProvider)
    {
        _bankTransactionHistoryData = bankTransactionHistoryData;
        _userData = userData;
        _authProvider = authProvider;
    }
    public Task ArchiveRecord(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecord(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<BankTransactionHistoryModel> GetRecordById(string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BankTransactionHistoryModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<BankTransactionHistoryModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public async Task<List<BankTransactionHistoryListDTO>> GetRecordsByDateRange(string bankId, DateTimeRange dateTimeRange)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            _recordsByDateRange = await _bankTransactionHistoryData.GetRecordsByDateRange(user.Id, bankId, dateTimeRange);

            return _recordsByDateRange;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<BankTransactionHistoryModel>> GetSearchResults(string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(BankTransactionHistoryModel model)
    {
        throw new NotImplementedException();
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return await _authProvider.GetUserFromAuth(_userData);
    }
}
