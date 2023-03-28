namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface ILocationData<T>
{
    Task SaveRecord(T model);
}
