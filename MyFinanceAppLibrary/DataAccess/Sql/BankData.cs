namespace MyFinanceAppLibrary.DataAccess.Sql;

public class BankData : IBankData
{
    private readonly IDataAccess _dataAccess;

    public BankData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<BankModel>> GetBanks(string userId)
    {
        return _dataAccess.LoadData<BankModel, dynamic>(
            "myfinancedb.spBank_GetAll",
            new { userId = userId },
            "Mysql");
    }

    public async Task<BankModel> GetBankById(string userId, string bankId)
    {
        var result = await _dataAccess.LoadData<BankModel, dynamic>(
            "myfinancedb.spBank_GetById",
            new { userId = userId, bankId = bankId },
            "Mysql");

        return result.FirstOrDefault()!;
    }

    public async Task CreateBank(BankModel bankModel)
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
        await Task.CompletedTask;
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _dataAccess.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public async Task UpdateBank(BankModel bankModel)
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
            
        await Task.CompletedTask;
    }
}
