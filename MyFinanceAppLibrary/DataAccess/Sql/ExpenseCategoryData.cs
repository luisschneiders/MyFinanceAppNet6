namespace MyFinanceAppLibrary.DataAccess.Sql;

public class ExpenseCategoryData : IExpenseCategoryData<ExpenseCategoryModel>
{
    private readonly IDataAccess _dataAccess;

    public ExpenseCategoryData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(ExpenseCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpenseCategory_Archive",
                new
                {
                    expenseCategoryId = model.Id,
                    expenseCategoryIsArchived = model.IsArchived,
                    expenseCategoryUpdatedBy = model.UpdatedBy,
                    expenseCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(ExpenseCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpenseCategory_Create",
                new
                {
                    expenseCategoryDescription = model.Description,
                    expenseCategoryColor = model.Color,
                    expenseCategoryUpdatedBy = model.UpdatedBy,
                    expenseCategoryCreatedAt = model.CreatedAt,
                    expenseCategoryUpdatedAt = model.UpdatedAt,
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

    public async Task<ExpenseCategoryModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<ExpenseCategoryModel, dynamic>(
                "myfinancedb.spExpenseCategory_GetById",
                new { userId = userId, expenseCategoryId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<ExpenseCategoryModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseCategoryModel, dynamic>(
                "myfinancedb.spExpenseCategory_GetAll",
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

    public async Task<List<ExpenseCategoryModel>> GetRecordsActive(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseCategoryModel, dynamic>(
                "myfinancedb.spExpenseCategory_GetAllActive",
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

    public async Task<List<ExpenseCategoryModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseCategoryModel, dynamic>(
                "myfinancedb.spExpenseCategory_GetSearchResults",
                new
                {
                    userId = userId,
                    searchExpenseCategory = search,
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

    public async Task UpdateRecord(ExpenseCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpenseCategory_Update",
                new
                {
                    expenseCategoryId = model.Id,
                    expenseCategoryDescription = model.Description,
                    expenseCategoryColor = model.Color,
                    expenseCategoryIsActive = model.IsActive,
                    expenseCategoryUpdatedBy = model.UpdatedBy,
                    expenseCategoryUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(ExpenseCategoryModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spExpenseCategory_UpdateStatus",
                new
                {
                    expenseCategoryId = model.Id,
                    expenseCategoryIsActive = model.IsActive,
                    expenseCategoryUpdatedBy = model.UpdatedBy,
                    expenseCategoryUpdatedAt = model.UpdatedAt,
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
