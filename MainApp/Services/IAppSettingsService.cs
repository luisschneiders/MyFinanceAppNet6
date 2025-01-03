﻿namespace MainApp.Services;

public interface IAppSettingsService
{
    public Task SetShapes(string radius);
    public Task SetTableColumns(string view, List<TableColumn> columns);
    public Task<string> GetShapes();
    public Task<AppSettings> GetInterface();
    public Task<string> GetLocalStorageStartOfWeek();
    public Task SetLocalStorageStartOfWeek(string startOfWeek);
    public Task<string> GetLocalStorageAppTheme();
    public Task SetLocalStorageAppTheme(string theme);
}
