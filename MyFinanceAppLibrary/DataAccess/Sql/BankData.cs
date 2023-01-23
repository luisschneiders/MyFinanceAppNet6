namespace MyFinanceAppLibrary.DataAccess.Sql;

public class BankData : IBankData
{
    private readonly IDataAccess _dataAccess;

    public BankData(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public Task<List<BankModel>> GetAllBanksByUserId(string userId)
    {
        return _dataAccess.LoadData<BankModel, dynamic>(
            "myfinancedb.spBank_GetAllBanksByUserId",
            new { userId = userId },
            "Mysql");
    }
}
