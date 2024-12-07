using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class ExpenseService : IExpenseService<ExpenseModel>
{
    [Inject]
    private IExpenseData<ExpenseModel> _expenseData { get; set; } = default!;

    [Inject]
    private ILocationExpenseService<LocationExpenseModel> _locationExpenseService { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    [Inject]
    ILocalStorageService _localStorageService{ get; set; } = default!;

    private List<ExpenseListDTO> _recordsByDateRange { get; set; } = new();

    private decimal _expensesByDateRangeSum { get; set; } = 0;

    public ExpenseService(
        IExpenseData<ExpenseModel> expenseData,
        ILocationExpenseService<LocationExpenseModel> locationExpenseService,
        IUserData userData,
        AuthenticationStateProvider authProvider,
        ILocalStorageService localStorageService)
    {
        _expenseData = expenseData;
        _locationExpenseService = locationExpenseService;
        _userData = userData;
        _authProvider = authProvider;
        _localStorageService = localStorageService;
    }

    public async Task ArchiveRecord(ExpenseModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.IsArchived = true;
            model.IsActive = false;
            model.UpdatedBy = user.Id;
            model.UpdatedAt = DateTime.Now;

            await _expenseData.ArchiveRecord(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(ExpenseModel model)
    {
        try
        {
            var user = await GetLoggedInUser();

            model.UpdatedBy = user.Id;

            await _expenseData.CreateRecord(model);
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

    public async Task<ExpenseModel> GetRecordById(string modelId)
    {
        try
        {
            var user = await GetLoggedInUser();
            ExpenseModel result = await _expenseData.GetRecordById(user.Id, modelId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<ExpenseModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public async Task<decimal> GetRecordsByDateRangeSum()
    {
        try
        {
            return await Task.FromResult(_expensesByDateRangeSum);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseByCategoryGroupDTO>> GetRecordsListView(MultiFilterExpenseDTO filter)
    {
        try
        {
            List<ExpenseListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<ExpenseListDTO> recordsFiltered = new();
            List<ExpenseByCategoryGroupDTO> resultsByGroup;

            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                resultsByGroup = await SetRecordsListView(recordsFiltered);
                _expensesByDateRangeSum = recordsFiltered.Sum(t => t.Amount);
            }
            else
            {
                resultsByGroup = await SetRecordsListView(records);
                _expensesByDateRangeSum = records.Sum(t => t.Amount);
            }

            return resultsByGroup;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseCalendarDTO>> GetRecordsCalendarView(MultiFilterExpenseDTO filter)
    {
        try
        {
            List<ExpenseListDTO> records = await GetRecordsByDateRange(filter.DateTimeRange);
            List<ExpenseListDTO> recordsFiltered = new();
            List<ExpenseCalendarDTO> calendarData = new();
            
            if (filter.IsFilterChanged is true)
            {
                recordsFiltered = await SetRecordsFilter(filter);
                _expensesByDateRangeSum = recordsFiltered.Sum(t => t.Amount);
                calendarData = await SetRecordsCalendarView(recordsFiltered);
            }
            else
            {
                _expensesByDateRangeSum = records.Sum(t => t.Amount);
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

    public async Task<List<ExpenseDetailsDTO>> GetRecordsDateView(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseDetailsDTO> records = await _expenseData.GetRecordsDetailsByDateRange(user.Id, dateTimeRange);

            return await Task.FromResult(records);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public Task<List<ExpenseModel>> GetSearchResults(string search)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecord(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecordStatus(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseLast3MonthsGraphDTO> results = await _expenseData.GetRecordsLast3Months(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseLast5YearsGraphDTO> results = await _expenseData.GetRecordsLast5Years(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseAmountHistoryDTO>> GetAmountHistory()
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseAmountHistoryDTO> results = await _expenseData.GetAmountHistory(user.Id);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<GoogleMapStaticModel> GetLocationExpense(
        DateTimeRange dateTimeRange,
        MapMarkerColor mapMarkerColor,
        MapSize mapSizeWidth,
        MapSize mapSizeHeight,
        MapScale scale)
    {
        try
        {
            List<LocationExpenseDTO> results = await _locationExpenseService.GetRecordsByDateRange(dateTimeRange);

            string location = await BuildLocation(results);

            GoogleMapStaticModel model = new()
            {
                Location = location,
                Marker = $"color:{mapMarkerColor.ToString().ToLower()}",
                Scale = scale,
                Width = mapSizeWidth,
                Height = mapSizeHeight
            };

            return model;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseTop5DTO>> GetRecordsTop5ByDate(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseTop5DTO> results = await _expenseData.GetTop5ExpensesByDateRange(user.Id, dateTimeRange);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseListGroupByMonthDTO>> GetRecordsGroupByMonth(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseListGroupByMonthDTO> results = await _expenseData.GetRecordsGroupByMonth(user.Id, dateTimeRange);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<string> GetLocalStorageViewType()
    {
        try
        {
            string? localStorage = await _localStorageService.GetAsync<string>(LocalStorage.AppExpenseView);

            return await Task.FromResult(localStorage!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task SetLocalStorageViewType(string view)
    {
        try
        {
            await _localStorageService.SetAsync<string>(LocalStorage.AppExpenseView, view);
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

    private static Task<string> BuildLocation(List<LocationExpenseDTO> locations)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var location in locations)
        {
            sb.Append($"{location.Latitude},");
            sb.Append($"{location.Longitude}|");
        }

        return Task.FromResult(sb.ToString());
    }

    private async Task<List<ExpenseListDTO>> GetRecordsByDateRange(DateTimeRange dateTimeRange)
    {
        try
        {
            var user = await GetLoggedInUser();

            _recordsByDateRange = await _expenseData.GetRecordsByDateRange(user.Id, dateTimeRange);

            return _recordsByDateRange;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    private async Task<List<ExpenseListDTO>> SetRecordsFilter(MultiFilterExpenseDTO filter)
    {
        try
        {
            if (filter.BankId.Count > 0 || filter.ECategoryId.Count > 0)
            {
                List<ExpenseListDTO> recordsFiltered = new();

                if (filter.BankId.Count > 0 && filter.ECategoryId.Count == 0) // Filter by Bank only
                {
                    recordsFiltered = _recordsByDateRange.Where(e => filter.BankId.Contains(e.BankId)).ToList();
                }
                else if (filter.BankId.Count == 0 && filter.ECategoryId.Count > 0) // Filter by Expense only
                {
                    recordsFiltered = _recordsByDateRange.Where(e => filter.ECategoryId.Contains(e.ECategoryId)).ToList();
                }
                else // Filter by Bank and Expense
                {
                    recordsFiltered = _recordsByDateRange.Where(e => filter.BankId.Contains(e.BankId) && 
                                                         filter.ECategoryId.Contains(e.ECategoryId)).ToList();
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

    private static async Task<List<ExpenseCalendarDTO>> SetRecordsCalendarView(List<ExpenseListDTO> records)
    {
        try
        {
            List<ExpenseCalendarDTO> results = new();

            foreach (var record in records)
            {
                ExpenseCalendarDTO expenseCalendarDTO = new();

                var result = results.Find(e => e.ECategoryDescription == record.ExpenseCategoryDescription &&
                                               e.EDate.Date == record.EDate.Date);

                if (result is not null)
                {
                    result.Amount += record.Amount;
                }
                else
                {
                    expenseCalendarDTO.EDate = record.EDate;
                    expenseCalendarDTO.ECategoryColor = record.ExpenseCategoryColor;
                    expenseCalendarDTO.ECategoryDescription = record.ExpenseCategoryDescription;
                    expenseCalendarDTO.Amount = record.Amount;
                    results.Add(expenseCalendarDTO);
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

    private static async Task<List<ExpenseByCategoryGroupDTO>> SetRecordsListView(List<ExpenseListDTO> records)
    {
        try
        {
            var resultsByGroup = records.GroupBy(ec => ec.ExpenseCategoryDescription);
            var results = resultsByGroup.Select(ecGroup => new ExpenseByCategoryGroupDTO()
            {
                Description = ecGroup.Key,
                Color = ecGroup.Select(c => c.ExpenseCategoryColor).FirstOrDefault(),
                Total = ecGroup.Sum(a => a.Amount),
                Expenses = ecGroup.ToList()
            }).ToList();

            return await Task.FromResult(results);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
