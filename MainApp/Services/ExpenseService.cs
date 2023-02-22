using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MainApp.Services;

public class ExpenseService : IExpenseService<ExpenseModel>
{
    [Inject]
    private IExpenseData<ExpenseModel> _expenseData { get; set; } = default!;

    [Inject]
    private IBankData<BankModel> _bankData { get; set; } = default!;

    [Inject]
    private IExpenseCategoryData<ExpenseCategoryModel> _expenseCategoryData { get; set; } = default!;

    [Inject]
    private AuthenticationStateProvider _authProvider { get; set; } = default!;

    [Inject]
    private IUserData _userData { get; set; } = default!;

    private UserModel _loggedInUser { get; set; } = new();

    public ExpenseService(
        IExpenseData<ExpenseModel> expenseData,
        IBankData<BankModel> bankData,
        IExpenseCategoryData<ExpenseCategoryModel> expenseCategoryData,
        IUserData userData,
        AuthenticationStateProvider authProvider)
    {
        _expenseData = expenseData;
        _bankData = bankData;
        _expenseCategoryData = expenseCategoryData;
        _userData = userData;
        _authProvider = authProvider;
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

    public async Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        try
        {
            var user = await GetLoggedInUser();
            List<ExpenseModelListDTO> results = await _expenseData.GetRecordsByDateRange(user.Id, dateTimeRangeModel);
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseModelByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        try
        {
            var records = await GetRecordsByDateRange(dateTimeRangeModel);
            var resultsGroupBy = records.GroupBy(tc => tc.ExpenseCategoryDescription);
            var results = resultsGroupBy.Select(tcGroup => new ExpenseModelByCategoryGroupDTO()
            {
                Description = tcGroup.Key,
                Total = tcGroup.Sum(a => a.Amount),
                Expenses = tcGroup.ToList()
            }).ToList();

            return results;
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

    private async Task<UserModel> GetLoggedInUser()
    {
        return _loggedInUser = await _authProvider.GetUserFromAuth(_userData);
    }
}
