namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IDataAccess
{
    Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
    Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
}
