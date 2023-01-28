namespace MyFinanceAppLibrary.DataAccess.Sql;

public class BankData : IBankData
{
    private readonly IDataAccess _dataAccess;

    public BankData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<BankModel>> GetBanks(string userId)
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

    public async Task<BankModel> GetBankById(string userId, string bankId)
    {
        try
        {
            var result = await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_GetById",
                new { userId = userId, bankId = bankId },
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

    public async Task CreateBank(BankModel bankModel)
    {
        try
        {
            await _dataAccess.LoadData<BankModel, dynamic>(
                "myfinancedb.spBank_Create",
                new {
                    bankAccount = bankModel.Account,
                    bankDescription = bankModel.Description,
                    bankInitialBalance = bankModel.InitialBalance,
                    bankCurrentBalance = bankModel.CurrentBalance,
                    bankUpdatedBy = bankModel.UpdatedBy,
                    bankCreatedAt = bankModel.CreatedAt,
                    bankUpdatedAt = bankModel.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateBank(BankModel bankModel)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spBank_Update",
                new
                {
                    bankId = bankModel.Id,
                    bankAccount = bankModel.Account,
                    bankDescription = bankModel.Description,
                    bankCurrentBalance = bankModel.CurrentBalance,
                    bankIsActive = bankModel.IsActive,
                    bankUpdatedBy = bankModel.UpdatedBy,
                    bankUpdatedAt = bankModel.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task UpdateBankStatus(BankModel bankModel)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spBank_UpdateStatus",
                new
                {
                    bankId = bankModel.Id,
                    bankIsActive = bankModel.IsActive,
                    bankUpdatedBy = bankModel.UpdatedBy,
                    bankUpdatedAt = bankModel.UpdatedAt,
                },
                "Mysql");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task ArchiveBank(BankModel bankModel)
    {
        try
        {
            await _dataAccess.SaveData<dynamic>(
                "myfinancedb.spBank_Archive",
                new
                {
                    bankId = bankModel.Id,
                    bankIsArchived = bankModel.IsArchived,
                    bankUpdatedBy = bankModel.UpdatedBy,
                    bankUpdatedAt = bankModel.UpdatedAt,
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
