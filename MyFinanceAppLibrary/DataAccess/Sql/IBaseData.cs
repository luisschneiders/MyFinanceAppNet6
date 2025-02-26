namespace MyFinanceAppLibrary.DataAccess.Sql;

public interface IBaseData<T>
{
    public Task<List<T>> GetRecords(string userId);
    public Task<List<T>> GetRecordsActive(string userId);
    public Task<List<T>> GetSearchResults(string userId, string search);
    public Task<T> GetRecordById(string userId, string modelId);
    public Task<ulong> GetLastInsertedId();
    public Task CreateRecord(T model);
    public Task UpdateRecord(T model);
    public Task UpdateRecordStatus(T model);
    public Task ArchiveRecord(T model);
}
