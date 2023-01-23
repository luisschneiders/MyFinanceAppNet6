namespace MyFinanceAppLibrary.DataAccess.Sql;

public class BankData : IBankData
{
    private readonly IDataAccess _dataAccess;

    public BankData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<BankModel>> GetAllBanks(string userId)
    {
        return _dataAccess.LoadData<BankModel, dynamic>(
            "myfinancedb.spBank_GetAllBanks",
            new { userId = userId },
            "Mysql");
    }

    public async Task<BankModel> GetBankById(string userId, string bankId)
    {
        var results = await _dataAccess.LoadData<BankModel, dynamic>(
            "myfinancedb.spBank_GetBankById",
            new { userId = userId, bankId = bankId },
            "Mysql");

        return results.FirstOrDefault()!;
    }
}
