using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyFinanceAppLibrary.DataAccess.Sql;

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

    public Task ArchiveRecord(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public Task CreateRecord(ExpenseModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
    }

    public Task<ExpenseModel> GetRecordById(string modelId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetRecords()
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetRecordsActive()
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModelByCategoryGroupDTO>> GetRecordsByGroupAndDateRange(DateTimeRangeModel dateTimeRangeModel)
    {
        throw new NotImplementedException();
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
}
