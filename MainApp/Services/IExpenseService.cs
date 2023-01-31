namespace MainApp.Services;

public interface IExpenseService
{
    Task<List<ExpenseModel>> GetExpenses();
}
