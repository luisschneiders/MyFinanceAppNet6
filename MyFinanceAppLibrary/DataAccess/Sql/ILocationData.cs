namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ILocationData<T>
{
    public Task<LocationModel> GetRecordById(string userId);
    public Task SaveRecord(T model);
}
