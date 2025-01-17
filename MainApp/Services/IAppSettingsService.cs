namespace MainApp.Services;

public interface IAppSettingsService
{
    public Task<AppSettings> GetInterface();
    public Task SetShapes(string radius);
    public Task<string> GetShapes();
    public Task SetShadow(string shadow);
    public Task<string> GetShadow();
    public Task SetTableColumns(string view, List<TableColumn> columns);
    public Task SetLocalStorageStartOfWeek(string startOfWeek);
    public Task<string> GetLocalStorageStartOfWeek();
    public Task SetLocalStorageAppTheme(string theme);
    public Task<string> GetLocalStorageAppTheme();
}
