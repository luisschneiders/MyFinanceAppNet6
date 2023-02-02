namespace MainApp.Services;

public interface IBaseService<T>
{
    Task<List<T>> GetRecords();
    Task<List<T>> GetSearchResults(string search);
    Task<T> GetRecordById(string modelId);
    Task<ulong> GetLastInsertedId();
    Task CreateRecord(T model);
    Task UpdateRecord(T model);
    Task UpdateRecordStatus(T model);
    Task ArchiveRecord(T model);
}
