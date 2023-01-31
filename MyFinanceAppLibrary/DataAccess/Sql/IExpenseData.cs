namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IExpenseData
{
    Task<List<ExpenseModel>> GetExpenses(string userId);
}
