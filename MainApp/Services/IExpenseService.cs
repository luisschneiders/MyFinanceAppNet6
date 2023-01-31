namespace MainApp.Services;

public interface IExpenseService
{
    Task<List<ExpenseModel>> GetExpenses();
    Task<List<ExpenseModel>> GetSearchResults(string search);
    Task<ExpenseModel> GetExpenseById(string modelId);
    Task<ulong> GetLastInsertedId();
    Task CreateExpense(ExpenseModel model);
    Task UpdateExpense(ExpenseModel model);
    Task UpdateExpenseStatus(ExpenseModel model);
    Task ArchiveExpense(ExpenseModel model);
}
