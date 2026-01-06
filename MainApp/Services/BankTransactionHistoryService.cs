
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

    public async Task<List<BankTransactionHistoryByDateGroupDTO>> GetRecordsListView(MultiFilterBankTransactionHistoryDTO filter)
    {
        try
        {
            List<BankTransactionHistoryListDTO> records = await GetRecordsByBankLoadMore(filter);
            List<BankTransactionHistoryListDTO> recordsFiltered = new();
            List<BankTransactionHistoryByDateGroupDTO> results = new();

            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                results = await SetRecordsListView(recordsFiltered);
            }
            else
            {
                results = await SetRecordsListView(records);
            }

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<BankTransactionHistoryListDTO>> GetRecordsByBankLoadMore(MultiFilterBankTransactionHistoryDTO filter)
    {
        try
        {
            UserModel user = await GetLoggedInUser();

            _recordsByDateRange = await _bankTransactionHistoryData.GetRecordsByBankLoadMore(user.Id, filter.BankId, filter.LastId, filter.LoadMore);

            return _recordsByDateRange;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<BankTransactionHistoryListDTO>> SetRecordsFilter(MultiFilterBankTransactionHistoryDTO filter)
    {
        try
        {
            if (filter.Action.Count > 0)
            {
                List<BankTransactionHistoryListDTO> recordsFiltered = new();

                recordsFiltered = _recordsByDateRange.Where(bth => filter.Action.Contains(bth.ActionDescription)).ToList();

                return await Task.FromResult(recordsFiltered);
            }
            else
            {
                return await Task.FromResult(_recordsByDateRange);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<BankTransactionHistoryByDateGroupDTO>> SetRecordsListView(List<BankTransactionHistoryListDTO> records)
    {
        try
        {
            var resultsByGroup = records.GroupBy(tc => tc.BDate);

            var results = resultsByGroup.Select(tcGroup => new BankTransactionHistoryByDateGroupDTO()
            {
                BDate = tcGroup.Key,

                Transactions = tcGroup.ToList()
            }).ToList();

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<UserModel> GetLoggedInUser()
    {
        return await _authProvider.GetUserFromAuth(_userData);
    }
}
