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
        var result = await _dataAccess.LoadData<BankModel, dynamic>(
            "myfinancedb.spBank_Create",
            new {
                Account = bankModel.Account,
                Description = bankModel.Description,
                InitialBalance = bankModel.InitialBalance,
                CurrentBalance = bankModel.CurrentBalance,
                UpdatedBy = bankModel.UpdatedBy,
                CreatedAt = bankModel.CreatedAt,
                UpdatedAt = bankModel.UpdatedAt,
            },
            "Mysql");
        await Task.CompletedTask;
    }

    public async Task<ulong> GetLastInsertedId()
    {
        var lastInsertedId = await _dataAccess.GetLastInsertedId();
        return await Task.FromResult(lastInsertedId);
    }

    public Task<BankModel> UpdateBank(BankModel bankModel)
    {
        throw new NotImplementedException();
    }
}
