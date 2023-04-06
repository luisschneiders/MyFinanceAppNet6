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
                    expenseBankId = model.BankId,
                    expenseTransactionId = model.TransactionId,
                    expenseAmount = model.Amount,
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
            await _dataAccess.SaveData<dynamic>(
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

    public async Task<List<ExpenseListDTO>> GetRecordsByDateRange(string userId, DateTimeRange dateTimeRange)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseListDTO, dynamic>(
                "myfinancedb.spExpense_GetRecordsByDateRange",
                new
                {
                    userId = userId,
                    startDate = dateTimeRange.Start,
                    endDate = dateTimeRange.End
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

    public async Task<List<ExpenseLast3MonthsGraphDTO>> GetRecordsLast3Months(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseLast3MonthsGraphDTO, dynamic>(
                "myfinancedb.spExpense_GetRecordsSumLast3Months",
                new
                {
                    userId = userId,
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

    public async Task<List<ExpenseLast5YearsGraphDTO>> GetRecordsLast5Years(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<ExpenseLast5YearsGraphDTO, dynamic>(
                "myfinancedb.spExpense_GetRecordsSumLast5Years",
                new
                {
                    userId = userId,
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
