namespace MyFinanceAppLibrary.DataAccess.Sql;

public class ExpenseData : IExpenseData
{
    private readonly IDataAccess _dataAccess;

    public ExpenseData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<ExpenseModel>> GetExpenses(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseModel, dynamic>(
                "myfinancedb.spExpense_GetAll",
                new { userId = userId },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
