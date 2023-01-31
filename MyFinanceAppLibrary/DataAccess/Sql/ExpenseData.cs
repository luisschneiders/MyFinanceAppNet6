namespace MyFinanceAppLibrary.DataAccess.Sql;

public class ExpenseData : IExpenseData<ExpenseModel>
{
    private readonly IDataAccess _dataAccess;

    public ExpenseData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(ExpenseModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpense_Archive",
            new
            {
                expenseId = model.Id,
                expenseIsArchived = model.IsArchived,
                expenseUpdatedBy = model.UpdatedBy,
                expenseUpdatedAt = model.UpdatedAt,
            },
            "Mysql");
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
            await _dataAccess.LoadData<ExpenseModel, dynamic>(
                "myfinancedb.spExpense_Create",
                new
                {
                    expenseDescription = model.Description,
                    expenseUpdatedBy = model.UpdatedBy,
                    expenseCreatedAt = model.CreatedAt,
                    expenseUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _dataAccess.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task<ExpenseModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<ExpenseModel, dynamic>(
                "myfinancedb.spExpense_GetById",
                new { userId = userId, expenseId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseModel>> GetRecords(string userId)
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

    public async Task<List<ExpenseModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseModel, dynamic>(
                "myfinancedb.spExpense_GetSearchResults",
                new
                {
                    userId = userId,
                    searchExpense = search,
                },
                "Mysql");

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecord(ExpenseModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpense_Update",
                new
                {
                    expenseId = model.Id,
                    expenseDescription = model.Description,
                    expenseIsActive = model.IsActive,
                    expenseUpdatedBy = model.UpdatedBy,
                    expenseUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(ExpenseModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpense_UpdateStatus",
                new
                {
                    expenseId = model.Id,
                    expenseIsActive = model.IsActive,
                    expenseUpdatedBy = model.UpdatedBy,
                    expenseUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }
}
