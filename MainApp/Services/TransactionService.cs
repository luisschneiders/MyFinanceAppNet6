using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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

    private List<TransactionListDTO> _recordsByDateRange { get; set; } = new();

    public TransactionService(
        ITransactionData<TransactionModel> transactionData,
        IUserData userData,
        AuthenticationStateProvider authProvider)
    {
        _transactionData = transactionData;
        _userData = userData;
        _authProvider = authProvider;
    }

    public async Task ArchiveRecord(TransactionModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.IsArchived = true;
            model.IsActive = false;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            switch (model.Label)
            {
                case "T":
                    await _transactionData.ArchiveRecordTransfer(model);
                    break;
                case "D":
                    await _transactionData.ArchiveRecordDebit(model);
                    break;
                case "C":
                    await _transactionData.ArchiveRecordCredit(model);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
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
            model.Label = model.TransactionCategoryModel.ActionType;
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
            model.Label = model.TransactionCategoryModel.ActionType;
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

            model.Label = model.TransactionCategoryModel.ActionType;
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

    public async Task<List<TransactionIOGraphByMonthDTO>> GetIOByDateRangeGroupByMonth(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TransactionIOGraphByMonthDTO> results = await _transactionData.GetIOByDateRangeGroupByMonth(user.Id, dateTimeRange);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TransactionIOGraphByDayDTO>> GetIOByDateRangeGroupByDay(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TransactionIOGraphByDayDTO> results = await _transactionData.GetIOByDateRangeGroupByDay(user.Id, dateTimeRange);
            return results;
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

    public async Task<TransactionModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            TransactionModel result = await _transactionData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
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

    public async Task<List<TransactionIOLast3MonthsGraphDTO>> GetRecordsLast3Months()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<TransactionIOLast3MonthsGraphDTO> results = await _transactionData.GetRecordsLast3Months(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<TransactionByCategoryGroupDTO>> GetRecordsListView(FilterTransactionDTO filter)
    {
        try
        {
            List<TransactionListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<TransactionListDTO> recordsFiltered = new();
            List<TransactionByCategoryGroupDTO> results = new();

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

    public async Task<List<TransactionCalendarDTO>> GetRecordsCalendarView(FilterTransactionDTO filter)
    {
        try
        {
            List<TransactionListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<TransactionListDTO> recordsFiltered = new();
            List<TransactionCalendarDTO> calendarData = new();
            
            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                calendarData = await SetRecordsCalendarView(recordsFiltered);
            }
            else
            {
                calendarData = await SetRecordsCalendarView(records);
            }

            return calendarData;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TransactionListDTO>> SetRecordsFilter(FilterTransactionDTO filter)
    {
        try
        {
            if (filter.FromBank > 0 || filter.TCategoryId > 0)
            {
                List<TransactionListDTO> recordsFiltered = new();

                if (filter.FromBank > 0 && filter.TCategoryId == 0) // Filter by Bank only
                {
                    recordsFiltered = _recordsByDateRange.Where(t => t.FromBank == filter.FromBank).ToList();
                }
                else if (filter.FromBank == 0 && filter.TCategoryId > 0) // Filter by Transaction only
                {
                    recordsFiltered = _recordsByDateRange.Where(t => t.TCategoryId == filter.TCategoryId).ToList();
                }
                else // Filter by Bank and Expense
                {
                    recordsFiltered = _recordsByDateRange.Where(t => t.FromBank == filter.FromBank && 
                                                         t.TCategoryId == filter.TCategoryId).ToList();
                }

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

    private static async Task<List<TransactionCalendarDTO>> SetRecordsCalendarView(List<TransactionListDTO> records)
    {
        try
        {
            List<TransactionCalendarDTO> results = new();

            foreach (var record in records)
            {
                TransactionCalendarDTO transactionCalendarDTO = new();

                var result = results.Find(t => (t.TCategoryDescription?.Length > 0 ? t.TCategoryDescription: "Expenses") == (record.TCategoryDescription?.Length > 0 ? record.TCategoryDescription: "Expenses") &&
                                               t.TDate.Date == record.TDate.Date);

                if (result is not null)
                {
                    result.Amount += record.Amount;
                }
                else
                {
                    transactionCalendarDTO.TDate = record.TDate;
                    transactionCalendarDTO.TCategoryColor = await GetTransactionLabelColor(record);
                    transactionCalendarDTO.TCategoryDescription = record.TCategoryDescription?.Length > 0 ? record.TCategoryDescription: "Expenses";
                    transactionCalendarDTO.Amount = record.Amount;
                    results.Add(transactionCalendarDTO);
                }
            }

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<string> GetTransactionLabelColor(TransactionListDTO record)
    {
        try
        {
            string color = string.Empty;

            switch (record.Label)
            {
                case "C":
                    color = TransactionTypeColor.Credit;
                    break;
                case "D":
                    color = TransactionTypeColor.Debit;
                    break;
                case "T":
                    color = TransactionTypeColor.Transfer;
                    break;
            }

            return await Task.FromResult(color);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<TransactionListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();

            _recordsByDateRange = await _transactionData.GetRecordsByDateRange(user.Id, dateTimeRange);

            return _recordsByDateRange;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private static async Task<List<TransactionByCategoryGroupDTO>> SetRecordsListView(List<TransactionListDTO> records)
    {
        try
        {            
            var resultsByGroup = records.GroupBy(tc => tc.TCategoryDescription);
            var results = resultsByGroup.Select(tcGroup => new TransactionByCategoryGroupDTO()
            {
                Description = tcGroup.Key?.Length > 0 ? tcGroup.Key : "Expenses",
                Total = tcGroup.Sum(a => a.Amount),
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
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}
