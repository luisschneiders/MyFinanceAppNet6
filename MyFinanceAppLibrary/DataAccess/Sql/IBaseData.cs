namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBaseData<T>
{
    Task<List<T>> GetRecords(string userId);
    Task<List<T>> GetRecordsActive(string userId);
    Task<List<T>> GetSearchResults(string userId, string search);
    Task<T> GetRecordById(string userId, string modelId);
    Task<ulong> GetLastInsertedId();
    Task CreateRecord(T model);
    Task UpdateRecord(T model);
    Task UpdateRecordStatus(T model);
    Task ArchiveRecord(T model);
}
