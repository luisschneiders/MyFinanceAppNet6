namespace MainApp.Services;

public interface IBaseService<T>
{
    public Task<List<T>> GetRecords();
    public Task<List<T>> GetRecordsActive();
    public Task<List<T>> GetSearchResults(string search);
    public Task<T> GetRecordById(string modelId);
    public Task<ulong> GetLastInsertedId();
    public Task CreateRecord(T model);
    public Task UpdateRecord(T model);
    public Task UpdateRecordStatus(T model);
    public Task ArchiveRecord(T model);
}
