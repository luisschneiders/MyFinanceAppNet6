namespace MyFinanceAppLibrary.DataAccess.Sql;

public class BankData : IBankData<BankModel>
{
    private readonly IDataAccess _dataAccess;

    public BankData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task ArchiveRecord(BankModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spBank_Archive",
                new
                {
                    bankId = model.Id,
                    bankIsArchived = model.IsArchived,
                    bankUpdatedBy = model.UpdatedBy,
                    bankUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task CreateRecord(BankModel model)
    {
        try
        {
            await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_Create",
                new
                {
                    bankAccount = model.Account,
                    bankDescription = model.Description,
                    bankInitialBalance = model.InitialBalance,
                    bankCurrentBalance = model.CurrentBalance,
                    bankUpdatedBy = model.UpdatedBy,
                    bankCreatedAt = model.CreatedAt,
                    bankUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<BankModelBalanceSumDTO> GetBankBalancesSum(string userId)
    {
        try
        {
            var result = await _dataAccess.LoadData<BankModelBalanceSumDTO, dynamic>(
                "myfinancedb.spBank_GetBalancesSum",
                new
                {
                    userId = userId
                },
                "Mysql");

            return result.FirstOrDefault()!;
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

    public async Task<BankModel> GetRecordById(string userId, string modelId)
    {
        try
        {
            var result = await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_GetById",
                new { userId = userId, bankId = modelId },
                "Mysql");

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task<List<BankModel>> GetRecords(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_GetAll",
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

    public async Task<List<BankModel>> GetRecordsActive(string userId)
    {
        try
        {
            var results = await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_GetAllActive",
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

    public async Task<List<BankModel>> GetSearchResults(string userId, string search)
    {
        try
        {
            var results = await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_GetSearchResults",
                new
                {
                    userId = userId,
                    searchBank = search,
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

    public async Task UpdateRecord(BankModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spBank_Update",
                new
                {
                    bankId = model.Id,
                    bankAccount = model.Account,
                    bankDescription = model.Description,
                    bankCurrentBalance = model.CurrentBalance,
                    bankIsActive = model.IsActive,
                    bankUpdatedBy = model.UpdatedBy,
                    bankUpdatedAt = model.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateRecordStatus(BankModel model)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spBank_UpdateStatus",
                new
                {
                    bankId = model.Id,
                    bankIsActive = model.IsActive,
                    bankUpdatedBy = model.UpdatedBy,
                    bankUpdatedAt = model.UpdatedAt,
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
