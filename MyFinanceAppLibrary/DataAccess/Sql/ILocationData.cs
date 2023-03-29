namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ILocationData<T>
{
    Task<LocationModel> GetRecordById(string userId);
    Task SaveRecord(T model);
}
