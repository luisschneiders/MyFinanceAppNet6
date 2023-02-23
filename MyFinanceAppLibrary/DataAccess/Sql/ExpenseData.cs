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
                    expenseIsActive = model.IsActive,
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
                    expenseEDate = model.EDate,
                    expenseBankId = model.BankId,
                    expenseECategoryId = model.ECategoryId,
                    expenseComments = model.Comments,
                    expenseAmount = model.Amount,
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

    public Task<ulong> GetLastInsertedId()
    {
        throw new NotImplementedException();
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

    public Task<List<ExpenseModel>> GetRecords(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<ExpenseModel>> GetRecordsActive(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ExpenseModelListDTO>> GetRecordsByDateRange(string userId, DateTimeRangeModel dateTimeRangeModel)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseModelListDTO, dynamic>(
                "myfinancedb.spExpense_GetRecordsByDateRange",
                new
                {
                    userId = userId,
                    startDate = dateTimeRangeModel.Start,
                    endDate = dateTimeRangeModel.End
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

    public Task<List<ExpenseModel>> GetSearchResults(string userId, string search)
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
